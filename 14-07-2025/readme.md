# 🔐 Azure Key Vault Integration in .NET

This guide explains how to securely store and access secrets like **Azure Blob Storage SAS URL** and **Database Connection Strings** from **Azure Key Vault** using a .NET Web API application.

---

## 🏗️ Create Key Vault and Secrets

![Key Vault](./images/KeyVault.png)


## 🔧 Install NuGet Packages

```
dotnet add package Azure.Security.KeyVault.Secrets
dotnet add package Azure.Identity
```

## 🧠 Configure appsettings.json

```
{
  "Azure": {
    "KeyVault": "https://<KeyVaultName>.vault.azure.net/"
  }
}
```

## 🧑‍💻 Load Secrets from Key Vault in Program.

![DB Code](./images/DatabaseCode.png)


## 🖼️ Uploading to Azure Blob using SAS

![File Upload Code](./images/FileUploadCode.png)


## 🔑 Secrets in Azure Key Vault

![Secret](./images/SecretList.png)

![Secret](./images/DBSecret.png)

![Secret](./images/SasSecret.png)

## 📌 Output

![Backend API](./images/BackendAPI.png)

![File Upload API](./images/FileUploadAPI.png)

![Upload List](./images/ContainerFiles.png)

