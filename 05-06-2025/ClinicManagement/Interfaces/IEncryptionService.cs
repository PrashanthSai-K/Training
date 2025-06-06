using System;
using System.Security.Cryptography.Xml;
using FirstAPI.Models;

namespace ClinicManagement.Interfaces;

public interface IEncryptionService
{
    Task<EncryptModel> EncryptData(EncryptModel encryptModel);
}
