# Deployment Instructions & Notes

## CLI only
```bash
# General settings
export RG="rg-lab"
export LOCATION="eastus"

# Keyvault settings
export VAULT_NAME="kv-digitalstoic-lab"

# Registry settings
export REGISTRY_NAME="digitalstoiclab"
export REGISTRY_URL="${REGISTRY_NAME}.azurecr.io"

# App settings
export APP_NAME="my-company.my-app"
export APP_MODULE="my-order.application"
export APP_MODULE_VERSION="0.1"
export APP_MODULE_FULLNAME="${APP_NAME}/${APP_MODULE}:${APP_MODULE_VERSION}"

# Identity settings
export REGISTRY_PULL_ID_NAME="id-${REGISTRY_NAME}-pull"

# Secret settings
export REGISTRY_PULL_SECRET_NAME="${REGISTRY_NAME}-pull"

# Container settings
export CONTAINER_NAME="ci-my-order-applicatíon"

az group create \
	--name $RG \
	--location $LOCATION

az keyvault create \
	--name $VAULT_NAME \
	--resource-group $RG \
	--location $LOCATION \
	--sku standard

export REGISTRY_PULL_ID=$(az ad sp list --filter "displayname eq '${REGISTRY_PULL_ID_NAME}'" --query '[0].appId' -o tsv)

az acr create \
	--resource-group $RG \
	--name $REGISTRY_NAME \
	--sku Basic \
	--zone-redundancy Disabled

REGISTRY_ID=$(az acr show --name $REGISTRY_NAME --query id -o tsv)

az ad sp create-for-rbac \
	--name $REGISTRY_PULL_ID_NAME \
	--scopes $REGISTRY_ID \
	--role acrpull

# If lost password
az ad sp credential reset --name $REGISTRY_PULL_ID

# Password manually written in .pwd
az keyvault secret set \
	--vault-name $VAULT_NAME \
	--name $REGISTRY_PULL_SECRET_NAME \
	--value $(cat .pwd)

az acr login --name $REGISTRY_NAME

docker push "${REGISTRY_URL}/${APP_MODULE_FULLNAME}"	

az container create \
    --name $CONTAINER_NAME \
    --resource-group $RG \
    --image "${REGISTRY_URL}/${APP_MODULE_FULLNAME}" \
	--registry-login-server $REGISTRY_URL \
	--registry-username $REGISTRY_PULL_ID \
	--registry-password $(az keyvault secret show --vault-name $VAULT_NAME -n $REGISTRY_PULL_SECRET_NAME --query value -o tsv)
	
```

## Mix Bicep / CLI

```bash
export LOC="eastus"
export RG="rg-lab"

export REGISTRY_NAME="digitalstoiclab"
export MODULE_IMAGE="my-company.my-app/my-order.application"
export MODULE_VERSION="0.1"

az identity create \
	--resource-group $RG \
	--name labId

spId=$(az identity show --resource-group $RG --name labId --query principalId --output tsv)
fullSpId=$(az identity show --resource-group $RG --name labId --query id --output tsv)

# Create container registry
az ts create \
	--name ts-container-registry \
	--version $MODULE_VERSION \
	--resource-group $RG \
	--location $LOC \
	--template-file deploy/container-registry.bicep

# TODO: Disable and use better authentication
# See https://learn.microsoft.com/en-us/azure/container-registry/container-registry-authentication?tabs=azure-cli
az acr update -n $REGISTRY_NAME --admin-enabled true

registry_template_id=$(az ts show --name ts-container-registry --resource-group $RG --version "0.1" --query "id")

az deployment group create \
	--resource-group $RG \
	--template-spec $registry_template_id

registry_id=$(az acr show --name digitalstoiclab --query id -o tsv)

# az role assignment create --assignee $spId --scope $registry_id --role acrpull

az acr login --name digitalstoiclab

docker push ${REGISTRY_NAME}.azurecr.io/${MODULE_IMAGE}:${MODULE_VERSION} 

# Create container instance
az ts create \
	--name ts-container-instance \
	--version $MODULE_VERSION \
	--resource-group $RG \
	--location $LOC \
	--template-file deploy/container-instance.bicep

container_template_id=$(az ts show --name ts-container-instance --resource-group $RG --version "0.1" --query "id")

az deployment group create \
	--resource-group $RG \
	--template-spec $container_template_id \
	--parameters containerGroupName="ci-lab-test" containerName="container-lab-test"

```
