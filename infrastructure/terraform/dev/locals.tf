locals {
  tags = {
    Environment = "Dev"
    Owner       = "Nazmy Achmad"
    Project     = "KHI Robocross"
    ManagedBy   = "Terraform"
  }

  dotnet_version = "7.0"

  # IP Range Filter here is to allow azure portal access
  cosmosdb_ip_range_azure = [
    "104.42.195.92/32",
    "40.76.54.131/32",
    "52.176.6.30/32",
    "52.169.50.45/32",
    "52.187.184.26/32"
  ]

  ip_whitelist = [
    "194.223.135.82/32"
  ]
}