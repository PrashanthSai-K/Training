using System;
using ClinicManagement.Models;

namespace ClinicManagement.Interfaces;

public interface ITokenService
{
    public Task<string> GenerateToken(User user, float YearsOfExperience);
    
}
