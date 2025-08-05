param location string = resourceGroup().location

param dbServerName string
@secure()
param dbAdmingUsername string
@secure()
param dbAdminPassword string

@minValue(32)
@maxValue(5120)
param dbStorageSize int = 128

@allowed(['B1ms'])
param dbSku string = 'B1ms'

param appConfigName string
param keyVaultName string
param managedIdentityName string

resource userIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: managedIdentityName
  location: location
}

resource postgresServer 'Microsoft.DBforPostgreSQL/flexibleServers@2023-03-01' = {
  name: dbServerName
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${userIdentity.id}': {}
    }
  }
  sku: {
    name: dbSku
    tier: 'GeneralPurpose'
    capacity: 2
    family: 'Gen5'
  }
  properties: {
    administratorLogin: dbAdmingUsername
    administratorLoginPassword: dbAdminPassword
    version: '15'
    storage: {
      dbStorageSize: dbStorageSize
      autoGrow: 'Enabled'
    }
    network: {
      publicNetworkAccess: 'Enabled'
    }
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    tenantId: subscription().tenantId
    sku: {
      name: 'standard'
      family: 'A'
    }
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: userIdentity.id
        permissions: {
          secrets: ['get', 'set', 'list']
        }
      }
    ]
    enabledForDeployment: true
    enablePurgeProtection: false
  }
}

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = {
  name: appConfigName
  location: location
  sku: {
    name: 'developer'
  }
}

resource dbPasswordSecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: keyVault
  name: 'PostgresAdminPassword'
  properties: {
    value: dbAdminPassword
  }
}

var dbConnStringTemplate = 'Host=${postgresServer.properties.fullyQualifiedDomainName};Database=postgres;Port=5432;Username=${dbAdmingUsername};Password=@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=PostgresAdminPassword);'

resource dbConnKV 'Microsoft.AppConfiguration/configurationStores/keyValues@2022-05-01' = {
  parent: appConfig
  name: 'DbConnectionString'
  properties: {
    value: dbConnStringTemplate
    contentType: 'text/plain'
  }
}
