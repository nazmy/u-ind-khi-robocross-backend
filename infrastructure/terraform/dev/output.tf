output "virtual_network_name" {
  value = azurerm_virtual_network.vnet.name
}

output "virtual_network_address_space" {
  value = azurerm_virtual_network.vnet.address_space
}

output "virtual_network_address_subnet" {
  value = azurerm_virtual_network.vnet.subnet
}

output "cosmosdb_mongo_name" {
  value = azurerm_cosmosdb_mongo_database.khirobocross.name
}

output "cosmosdb_mongo_throughput" {
  value = azurerm_cosmosdb_mongo_database.khirobocross.throughput
}

output "cosmosdb_mongo_auto_scale" {
  value = azurerm_cosmosdb_mongo_database.khirobocross.autoscale_settings
}