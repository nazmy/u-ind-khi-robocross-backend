data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "backend_rg" {
  name     = "${var.resource_prefix}-backend-rg"
  location = var.resource_location
  tags =  local.tags
}

resource "azurerm_resource_group" "frontend_rg" {
  name     = "${var.resource_prefix}-frontend-rg"
  location = var.resource_location
  tags = local.tags
}