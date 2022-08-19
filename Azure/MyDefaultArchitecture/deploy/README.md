# Deployment

## Setup 

```bash
az group list | jq '.[].id'
az group create --location eastus --resource-group rg-lab
az group list --query "[].[id,name,location]"
az group list --query "[].{id:id, name:name, location:location}"
z group list --query "[?name=='rg-lab'].[id,name,location]"
```