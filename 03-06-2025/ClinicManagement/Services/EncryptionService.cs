using System;
using System.Security.Cryptography;
using System.Text;
using ClinicManagement.Interfaces;
using FirstAPI.Models;

namespace ClinicManagement.Services;

public class EncryptionService : IEncryptionService
{
    public async Task<EncryptModel> EncryptData(EncryptModel encryptModel)
    {
        HMACSHA256 hMACSHA256;
        if (encryptModel.HashKey != null)
            hMACSHA256 = new HMACSHA256(encryptModel.HashKey);
        else
            hMACSHA256 = new HMACSHA256();
        encryptModel.EncryptedData = hMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(encryptModel.Data));
        encryptModel.HashKey = hMACSHA256.Key;
        return encryptModel;
    }
}
