using System;

namespace CustomerSupport.Interfaces;

public interface IHashingService
{
    public string HashData(string data);
    public bool VerifyHash(string data, string hashedData);
}
