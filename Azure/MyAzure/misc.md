# Miscellanous

## Requirements

[Install Azure Cli](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
```bash
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
```

[Install Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)
```bash
wget -q https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```

[Install Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=docker-hub)
```bash
docker pull mcr.microsoft.com/azure-storage/azurite
docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 \
    mcr.microsoft.com/azure-storage/azurite
```

## Azure CLI 101

```bash
az login
az login --use-device-code
az account list
az account show
```

### PowerShell

```powershell
Connect-AzAccount
Get-AzContext
Get-AzLocation
```

```powershell
$loc = 'southeastasia'
$rg = 'Lab'
New-AzResourceGroup -Name $rg -Location $loc
#  Remove-AzResourceGroup -Name $rg -Force
```