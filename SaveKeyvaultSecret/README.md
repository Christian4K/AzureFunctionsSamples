# SaveKeyvaultSecret

This sample writes the posted message into a connected keyvault by using a managed identity. This is originally used to store a token into a keyvault, which is than securly linked into an azure data factory, but shows the general process.

Required:
- Enabled managed identiy on azure function app
- Permission for managed identity on azure keyvault

For a description see:
https://www.v4kt.de/azure/keyvault-and-azure-functions/
