###############################
### Resources Variables
################################
variable "resource_prefix" {
  description = "Resource name prefix"
  default     = "Khirobocross"
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

variable "appservice_appinsight_instrumentation_key" {
  description = "App service app insight instrumentation key"
}
