output "virtual_network_name" {
  value = azurerm_virtual_network.vnet.name
}

output "virtual_network_address_space" {
  value = azurerm_virtual_network.vnet.address_space
}

output "virtual_network_address_subnet" {
  value = azurerm_virtual_network.vnet.subnet
}

output "storage_container_robot_configurator_name" {
  value = azurerm_storage_container.robot_configurator.name
}

output "storage_container_robot_monitoring_name" {
  value = azurerm_storage_container.robot_monitoring.name
}