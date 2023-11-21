# KHI Robocross Backend 

## Introduction
This repository stores following backend source code that are used in KHI Robocross project:
- [Backend Rest API] (https://github-vcs.unity3d.com/Solutions/ind-khi-robocross-backend/tree/main/Api)
- [Backend Domain] (https://github-vcs.unity3d.com/Solutions/ind-khi-robocross-backend/tree/main/Domain)
- [Azure Cloud Terraform] (https://github-vcs.unity3d.com/Solutions/ind-khi-robocross-backend/tree/main/infrastructure/terraform)

## Backend Services 
Robocross Backend service developed using c# .net core 7.0. It provides REST API to manage robocross
data, refer to [swagger](https://khirobocross-dev-api.azurewebsites.net/swagger/index.html) for the available endpoints.
The backend service uses MongoDB Database.

A .net 7.0 need to be installed before you can start the project. Follow the [instruction] (https://dotnet.microsoft.com/en-us/download/dotnet/7.0) on Microsoft website to install it.

Once .net core 7.0 is installed, follow below steps to run the project:
1. Clone this repository
2. (optional) Open the project in code editor (rider or vscode)
3. Open and modify the api/appsettings.json (you can also clone it to appsettings.Development.json)
4. Run `dotnet run` or setup the run configuration on your IDE
5. The API can be accessed on your localhost

## Terraform
KHI Robocross Azure Infrastructure is provisioned using [Terraform](https://www.terraform.io/). Terraform is enable us to codifies cloud infrastructure and provision Azure Cloud using code.
There are a few ways to install terraform. Please refer to [terraform documentation](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli)  for more information.
In MacOs, it's recommended to install `tfswitches` to allow switching between different Terraform versions:

```
❯ tfswitch
Use the arrow keys to navigate: ↓ ↑ → ← 
? Select Terraform version: 
  ▸ 1.5.4 *recent
    1.3.3 *recent
    1.3.1 *recent
    1.5.5
↓   1.5.3
```

The terraform state is stored in Unity team [Terraform Cloud] (https://app.terraform.io/session). Read the [instruction](https://docs.internal.unity.com/terraform-cloud/quickstart/access-tfc) on how to access TFC.
Below are the basic Terraform commands to initialize terraform and read terraform plan (apply isn't available since we're usinc TFC). 
Do read [terraform documentation](https://developer.hashicorp.com/terraform/cli/run) to understand mode details.
```
terraform init
terraform plan
```
