using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponseDto> Login(LoginRequestDto login);
}
