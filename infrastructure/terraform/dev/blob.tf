# storage for client misc (cloud bash etc)
resource "azurerm_storage_account" "misc_storage" {
  name                = replace("${var.resource_prefix}-misc-storage", "-", "")
  resource_group_name = azurerm_resource_group.backend_rg.name

  location                 = azurerm_resource_group.backend_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}


resource "azurerm_storage_container" "cloud_shell" {
  name                  = "cloud-shell"
  storage_account_name  = azurerm_storage_account.misc_storage.name
  container_access_type = "blob"
}
