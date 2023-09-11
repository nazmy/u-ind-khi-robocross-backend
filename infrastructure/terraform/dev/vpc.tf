########################
# Virtual Network
########################
resource "azurerm_virtual_network" "vnet" {
  name                = "${var.resource_prefix}-vnet"
  address_space       = ["10.0.0.0/16"]
  location            = azurerm_resource_group.backend_rg.location
  resource_group_name = azurerm_resource_group.backend_rg.name
}


resource "azurerm_subnet" "public" {
  name                 = "${var.resource_prefix}-public-subnet"
  resource_group_name  = azurerm_resource_group.backend_rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.1.0/24"]
}

resource "azurerm_subnet" "apim" {
  name                 = "${var.resource_prefix}-apim-subnet"
  resource_group_name  = azurerm_resource_group.backend_rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.2.0/24"]
}

resource "azurerm_subnet" "database" {
  name                 = "${var.resource_prefix}-database-subnet"
  resource_group_name  = azurerm_resource_group.backend_rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.3.0/24"]
  service_endpoints    = ["Microsoft.AzureCosmosDB"]


}

resource "azurerm_subnet" "private" {
  name                 = "${var.resource_prefix}-private-subnet"
  resource_group_name  = azurerm_resource_group.backend_rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.4.0/24"]
  service_endpoints    = ["Microsoft.Web", "Microsoft.AzureCosmosDB"]

  delegation {
    name = "app-service"
    service_delegation {
      name    = "Microsoft.Web/serverFarms"
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
    }
  }
}

