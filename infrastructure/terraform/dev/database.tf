#Cosmo DB for MonggoDB
resource "azurerm_cosmosdb_account" "cosmodb_account" {
  name                              = "${var.resource_prefix}-cosmos-mongodb"
  location                          = azurerm_resource_group.backend_rg.location
  resource_group_name               = azurerm_resource_group.backend_rg.name
  offer_type                        = "Standard"
  kind                              = "MongoDB"
  enable_automatic_failover         = var.mongodb_enable_automatic_failover
  is_virtual_network_filter_enabled = var.mongo_db_virtual_network_filter_enabled
  public_network_access_enabled     = var.mongodb_enable_public_access

  geo_location {
    location          = azurerm_resource_group.backend_rg.location
    failover_priority = 0
  }

  virtual_network_rule {
    id = azurerm_subnet.private.id
  }
  virtual_network_rule {
    id = azurerm_subnet.database.id
  }

  ip_range_filter = join(",", concat(local.cosmosdb_ip_range_azure, local.ip_whitelist))

  consistency_policy {
    consistency_level       = "Session"
    max_interval_in_seconds = 5
    max_staleness_prefix    = 100
  }
  depends_on = [
    azurerm_subnet.database
  ]
}


resource "azurerm_private_endpoint" "cosmodb_private_endpoint" {
  name                = "${var.resource_prefix}-cosmos-mongodb-private-endpoint"
  location            = azurerm_resource_group.backend_rg.location
  resource_group_name = azurerm_resource_group.backend_rg.name
  subnet_id           = azurerm_subnet.database.id

  private_service_connection {
    name                           = azurerm_cosmosdb_account.cosmodb_account.name
    private_connection_resource_id = azurerm_cosmosdb_account.cosmodb_account.id
    subresource_names              = ["MongoDB"]
    is_manual_connection           = false
  }
}

resource "azurerm_cosmosdb_mongo_database" "khirobocross" {
  name                = "${var.resource_prefix}-mongodb"
  resource_group_name = azurerm_resource_group.backend_rg.name
  account_name        = azurerm_cosmosdb_account.cosmodb_account.name

  autoscale_settings {
    max_throughput = var.mongodb_autoscale_max_throughput
  }
}