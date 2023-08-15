data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "backend_rg" {
  name     = "${var.resource_prefix}-backend-rg"
  location = var.resource_location
  tags     = local.tags
}

resource "azurerm_resource_group" "frontend_rg" {
  name     = "${var.resource_prefix}-frontend-rg"
  location = var.resource_location
  tags     = local.tags
}

## Azure App Service
resource "azurerm_service_plan" "khirobocross" {
  name                = "${var.resource_prefix}-service-plan"
  resource_group_name = azurerm_resource_group.backend_rg.name
  location            = azurerm_resource_group.backend_rg.location
  os_type             = "Linux"
  sku_name            = "B1" #for development only, change to Premium for staging/prod
}

resource "azurerm_linux_web_app" "khirobocross" {
  name                = "${var.resource_prefix}-web-app"
  resource_group_name = azurerm_resource_group.backend_rg.name
  location            = azurerm_service_plan.khirobocross.location
  service_plan_id     = azurerm_service_plan.khirobocross.id


  https_only            = true
  site_config { 
    minimum_tls_version = "1.2"
    application_stack {
      dotnet_version = local.dotnet_version
    } 
  }
}

resource "azurerm_app_service_source_control" "backend_scm" {
  app_id   = azurerm_linux_web_app.khirobocross.id
  repo_url = "https://github.com/Azure-Samples/dotnetcore-docs-hello-world"
  branch   = "master"
}