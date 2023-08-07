provider "azurerm" {
  features {}
}

terraform {
  required_version = "~> 1.5.4"
  backend "remote" {
    hostname     = "app.terraform.io"
    organization = "unity-technologies"
    workspaces {
      name = "ind-khi-robocross-backend-dev"
    }
  }
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.63.0"
    }
  }
}