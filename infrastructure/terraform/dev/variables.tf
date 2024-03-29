###############################
### Resources Variables
################################
variable "resource_prefix" {
  description = "Resource name prefix"
  default     = "khirobocross"
}

variable "resource_location" {
  description = "Resource location"
  default     = "Japan East"
}

################################
### MongoDB Variables
################################
variable "mongodb_enable_automatic_failover" {
  description = "is automatic failover on cosmo mongo DB enabled"
  default     = "false"
}

variable "mongo_db_virtual_network_filter_enabled" {
  description = "is virtual network filter enabled on mongoDB to allow access from subnets"
  default     = "true"
}

variable "mongodb_enable_public_access" {
  description = "is public access on cosmo mongo DB enabled"
  default     = "true"
}

variable "mongodb_autoscale_max_throughput" {
  description = "mongodb autoscale maximum throughput, between 1000 and 1,000,000"
  default     = 1000
}

################################
### Application Variables
################################

variable "aspnetcore_environment" {
  description = "ASP .Net Application Environment"
  default     = "Development"
}


variable "azure_mongodb_databasename" {
  description = "database name of azure cosmo mongo database"
  default     = "Khirobocross"
}

variable "cosmo_db_connection_string" {
  description = "Azure cosmo DB connection string"
}

//temp for Prod in dev subscription
variable "azure_devprod_mongodb_databasename" {
  description = "database name of azure cosmo mongo database for prod env in dev subscription"
  default     = "khirobocross-dev-prod"
}

variable "cosmo_devprod_db_connection_string" {
  description = "Azure cosmo DB connection string for prod env in dev subscription"
}
//end temp for Prod in dev subscription

//temp for irex
variable "azure_irex_mongodb_databasename" {
  description = "database name of azure cosmo mongo database for Irex"
  default     = "Khirobocross-irex"
}

variable "cosmo_irex_db_connection_string" {
  description = "Azure cosmo DB connection string for Irex"
}

variable "khirobocross_storage_connection_string" {
  description = "Azure KHI Robocross Storage connection string"
}

variable "khirobocross_storage_key" {
  description = "Azure KHI Robocross Storage Key"
}

variable "appservice_appinsight_instrumentation_key" {
  description = "App service app insight instrumentation key"
}

################################
### Environment
################################

variable "khirobocross_env" { 
  description = "KHI Robocross environment"
  default     = "dev"
}