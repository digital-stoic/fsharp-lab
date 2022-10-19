# Notes

```bash
# Env
export AZ_REGION="eastus"
export RESOURCE_GOUP="rg-lab"
export STORAGE_ACCOUNT="salabmyshopify"
export FUNCTION_APP="func-lab-myshopify"

# Build
dotnet build
func host start

# Deploy
az functionapp create --resource-group $RESOURCE_GROUP --consumption-plan-location $AZ_REGION --runtime dotnet --functions-version 4 --name $FUNCTION_APP --storage-account $STORAGE_ACCOUNT
func azure functionapp publish $FUNCTION_APP

# Clean
az functionapp delete --resource-group $RESOURCE_GROUP --name $FUNCTION_APP
```