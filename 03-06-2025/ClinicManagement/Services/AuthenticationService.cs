using System;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using FirstAPI.Models;

namespace ClinicManagement.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRepository<string, User> _userRepository;
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IEncryptionService _encryptionService;

    public AuthenticationService(IRepository<string, User> userRepository,
                                 IRepository<int, Doctor> doctorRepository,
                                 ITokenService tokenService,
                                 ILogger<AuthenticationService> logger,
                                 IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _doctorRepository = doctorRepository;
        _tokenService = tokenService;
        _logger = logger;
        _encryptionService = encryptionService;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto login)
    {
        var user = await _userRepository.GetById(login.Username);
        Console.WriteLine($"hii : {user.Password}, {login.Password}, {user.HashKey}");

        var encryptedData = await _encryptionService.EncryptData(
            new EncryptModel() { Data = login.Password, HashKey = user.HashKey });

        for (int i = 0; i < encryptedData?.EncryptedData?.Length; i++)
        {
            if (user?.Password?[i] != encryptedData.EncryptedData[i])
                throw new UnauthorizedAccessException("Invalid Password");
        }
        string token;
        if (user.Role == "Doctor")
        {
            var doctors = await _doctorRepository.GetAll();
            var doctor = doctors.FirstOrDefault(d => d.Email == user.UserName);
            token = await _tokenService.GenerateToken(user, doctor.YearsOfExperience);
        }
        else
        {
            token = await _tokenService.GenerateToken(user, 0);
        }

        return new LoginResponseDto()
        {
            Username = login.Username,
            Token = token
        };
    }
}
