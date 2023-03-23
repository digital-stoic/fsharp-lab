@description('Specifies the location for resources.')
param location string = 'eastus'

resource symbolicname 'Microsoft.ContainerRegistry/registries@2022-12-01' = {
  name: 'digitalstoicLab'
  location: location
  tags: {
    tagName1: 'notag'
  }
  sku: {
    name: 'Basic'
  }
  properties: {
    publicNetworkAccess: 'enabled'
    zoneRedundancy: 'disabled'
  }
}