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

#storage for client app assets
resource "azurerm_storage_account" "app_storage" {
  name                = replace("${var.resource_prefix}-app-storage", "-", "")
  resource_group_name = azurerm_resource_group.frontend_rg.name

  location                 = azurerm_resource_group.frontend_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "robot_configurator" {
  name                  = "robot-configurator"
  storage_account_name  = azurerm_storage_account.app_storage.name
  container_access_type = "blob"
}

resource "azurerm_storage_container" "robot_monitoring" {
  name                  = "robot-monitoring"
  storage_account_name  = azurerm_storage_account.app_storage.name
  container_access_type = "blob"
}


resource "azurerm_storage_account" "robot_library_storage" {
  name                = replace("${var.resource_prefix}-robot", "-", "")
  resource_group_name = azurerm_resource_group.frontend_rg.name

  location                 = azurerm_resource_group.frontend_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}


resource "azurerm_storage_container" "robot_library" {
  name                  = "robot-library"
  storage_account_name  = azurerm_storage_account.robot_library_storage.name
  container_access_type = "container"
}



