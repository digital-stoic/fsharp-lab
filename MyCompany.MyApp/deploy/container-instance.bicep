param containerGroupName string = 'ci-lab'
param location string = resourceGroup().location
param containerName string = 'container-lab'
param registryUrl string = 'digitalstoic.lab.azurecr.io'
param containerImage string = '{registryUrl}/my-company.my-app/my-order.application:0.1'
param spId string = '/subscriptions/09b6dbec-ff20-424d-b5b6-0535f433757a/resourcegroups/rg-lab/providers/Microsoft.ManagedIdentity/userAssignedIdentities/labId'

resource symbolicname 'Microsoft.ContainerInstance/containerGroups@2022-10-01-preview' = {
  name: containerGroupName
  location: location
  tags: {
    tagName1: 'notag'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    containers: [
      {
        name: containerName
        properties: {
          environmentVariables: [
            {
              name: 'VAR1'
              secureValue: 'var1_secure'
            }
          ]
          image: containerImage
          ports: [
            {
              port: 42
              protocol: 'tcp'
            }
          ]
          resources: {
            requests: {
              cpu: 1
              memoryInGB: json('1.0')
            }
          }
        }
      }
    ]
    imageRegistryCredentials: [
        {
            server: registryUrl
            identity: spId
        }
    ]
    osType: 'Linux'
    restartPolicy: 'Never'
    sku: 'Standard'
  }
  zones: [
    '1'
  ]
}