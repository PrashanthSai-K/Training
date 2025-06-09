using System;
using CustomerSupport.Models;

namespace CustomerSupport.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(User user);
    public string GenerateRefereshToken(User user);
    public bool CheckRefreshTokenValidity(string token);
}

