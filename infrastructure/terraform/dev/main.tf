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
  name                = "${var.resource_prefix}-${var.khirobocross_env}-api"
  resource_group_name = azurerm_resource_group.backend_rg.name
  location            = azurerm_service_plan.khirobocross.location
  service_plan_id     = azurerm_service_plan.khirobocross.id

  client_certificate_enabled = false
  client_certificate_mode    = "OptionalInteractiveUser"

  virtual_network_subnet_id = azurerm_subnet.private.id

  https_only = true

  app_settings = {
    "AZURE_MONGDB_DATABASENAME"      = var.azure_mongodb_databasename
    "ASPNETCORE_ENVIRONMENT"         = var.aspnetcore_environment
    "DOTNET_ENVIRONMENT"             = var.aspnetcore_environment
    "APPINSIGHTS_INSTRUMENTATIONKEY" = var.appservice_appinsight_instrumentation_key
  }

  connection_string {
    name  = "COSMOS_DB_CONNECTION_STRING"
    value = var.cosmo_db_connection_string
    type  = "Custom"
  }

  connection_string {
    name  = "KHIROBOCROSS_STORAGE_CONNECTION_STRING"
    value = var.khirobocross_storage_connection_string
    type  = "Custom"
  }

  connection_string {
    name  = "KHIROBOCROSS_STORAGE_KEY"
    value = var.khirobocross_storage_key
    type  = "Custom"
  }


  site_config {
    minimum_tls_version    = "1.2"
    vnet_route_all_enabled = true
    application_stack {
      dotnet_version = local.dotnet_version
    }
  }
}

//temp for Irex
resource "azurerm_linux_web_app" "khirobocross-irex" {
  name                = "${var.resource_prefix}-irex-api"
  resource_group_name = azurerm_resource_group.backend_rg.name
  location            = azurerm_service_plan.khirobocross.location
  service_plan_id     = azurerm_service_plan.khirobocross.id

  client_certificate_enabled = false
  client_certificate_mode    = "OptionalInteractiveUser"

  virtual_network_subnet_id = azurerm_subnet.private.id

  https_only = true

  app_settings = {
    "AZURE_MONGDB_DATABASENAME"      = var.azure_irex_mongodb_databasename
    "ASPNETCORE_ENVIRONMENT"         = var.aspnetcore_environment
    "DOTNET_ENVIRONMENT"             = var.aspnetcore_environment
    "APPINSIGHTS_INSTRUMENTATIONKEY" = var.appservice_appinsight_instrumentation_key
  }

  connection_string {
    name  = "COSMOS_DB_CONNECTION_STRING"
    value = var.cosmo_irex_db_connection_string
    type  = "Custom"
  }

  connection_string {
    name  = "KHIROBOCROSS_STORAGE_CONNECTION_STRING"
    value = var.khirobocross_storage_connection_string
    type  = "Custom"
  }

  connection_string {
    name  = "KHIROBOCROSS_STORAGE_KEY"
    value = var.khirobocross_storage_key
    type  = "Custom"
  }


  site_config {
    minimum_tls_version    = "1.2"
    vnet_route_all_enabled = true
    application_stack {
      dotnet_version = local.dotnet_version
    }
  }
}

//temp for auth
resource "azurerm_linux_web_app" "khirobocross-auth" {
  name                = "${var.resource_prefix}-auth-api"
  resource_group_name = azurerm_resource_group.backend_rg.name
  location            = azurerm_service_plan.khirobocross.location
  service_plan_id     = azurerm_service_plan.khirobocross.id

  client_certificate_enabled = false
  client_certificate_mode    = "OptionalInteractiveUser"

  virtual_network_subnet_id = azurerm_subnet.private.id

  https_only = true

  app_settings = {
    "AZURE_MONGDB_DATABASENAME"      = var.azure_mongodb_databasename
    "ASPNETCORE_ENVIRONMENT"         = var.aspnetcore_environment
    "DOTNET_ENVIRONMENT"             = var.aspnetcore_environment
    "APPINSIGHTS_INSTRUMENTATIONKEY" = var.appservice_appinsight_instrumentation_key
  }

  connection_string {
    name  = "COSMOS_DB_CONNECTION_STRING"
    value = var.cosmo_db_connection_string
    type  = "Custom"
  }

  connection_string {
    name  = "KHIROBOCROSS_STORAGE_CONNECTION_STRING"
    value = var.khirobocross_storage_connection_string
    type  = "Custom"
  }

  connection_string {
    name  = "KHIROBOCROSS_STORAGE_KEY"
    value = var.khirobocross_storage_key
    type  = "Custom"
  }


  site_config {
    minimum_tls_version    = "1.2"
    vnet_route_all_enabled = true
    application_stack {
      dotnet_version = local.dotnet_version
    }
  }
}