using System;
using ClinicManagement.Models.DTO;
using Microsoft.AspNetCore.Authentication;

namespace ClinicManagement.Interfaces;

public interface IGoogleAuthService
{
    Task<LoginResponseDto> AuthenticateUser(AuthenticateResult result); 
}
