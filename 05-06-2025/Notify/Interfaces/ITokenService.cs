using System;
using Notify.Models;

namespace Notify.Interfaces;

public interface ITokenService
{
    Task<string> GenerateToken(User user);

}
