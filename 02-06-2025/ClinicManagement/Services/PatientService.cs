using System;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagement.Interfaces;
using ClinicManagement.Mappers;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using FirstAPI.Models;

namespace ClinicManagement.Services;

public class PatientService : IPatientService
{
    private readonly IRepository<int, Patient> _patients;
    private readonly IRepository<string, User> _userRepository;
    private readonly PatientMapper _patientMapper;
    private readonly IMapper _mapper;
    private readonly IEncryptionService _encryptionService;

    public PatientService(IRepository<int, Patient> repository, IRepository<string, User> userRepository, IMapper mapper, IEncryptionService encryptionService)
    {
        _patients = repository;
        _userRepository = userRepository;
        _patientMapper = new();
        _mapper = mapper;
        _encryptionService = encryptionService;
    }
    public async Task<Patient> CreatePatient(PatientAddRequestDto patient)
    {
        var user =  _mapper.Map<PatientAddRequestDto, User>(patient);
        var encryptedData = await _encryptionService.EncryptData(new EncryptModel() { Data = patient.Password });
        user.Password = encryptedData.EncryptedData;
        user.HashKey = encryptedData.HashKey;
        user.Role = "Patient";
        user = await _userRepository.Create(user);

        var newpatient = _patientMapper.MapPatientAddRequestPatient(patient);
        await _patients.Create(newpatient);
        return newpatient;
    }

    public async Task<Patient> DeletePatient(int id)
    {
        var patient = await _patients.GetById(id);
        await _patients.Delete(id);
        return patient;
    }

    public async Task<Patient> GetById(int id)
    {
        return await _patients.GetById(id);
    }

    public async Task<IEnumerable<Patient>> GetPatients()
    {
        return await _patients.GetAll();
    }

    public async Task<Patient> UpdatePatient(Patient patient)
    {
        await _patients.Update(patient.Id, patient);
        return patient;
    }
}
