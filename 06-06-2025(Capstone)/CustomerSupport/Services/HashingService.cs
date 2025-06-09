using System;
using BCrypt.Net;
using CustomerSupport.Interfaces;

namespace CustomerSupport.Services;

public class HashingService : IHashingService
{
    public string HashData(string data)
    {
        var hashedData = BCrypt.Net.BCrypt.HashPassword(data);
        return hashedData;
    }

    public bool VerifyHash(string data, string hashedData)
    {
        return BCrypt.Net.BCrypt.Verify(data, hashedData);
    }
}
