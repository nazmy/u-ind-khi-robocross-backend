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

variable "is_mongo_db_virtual_network_filter_enabled" {
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

