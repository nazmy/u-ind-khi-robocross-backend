data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "khi_backend_rg" {
  name     = "${var.resource_prefix}-backend-rg"
  location = var.resource_location
}

resource "azurerm_resource_group" "khi_frontend_rg" {
  name     = "${var.resource_prefix}-frontend-rg"
  location = var.resource_location
}