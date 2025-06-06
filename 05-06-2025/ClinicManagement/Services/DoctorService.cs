using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagement.Interfaces;
using ClinicManagement.Mappers;
using ClinicManagement.Misc.OtherFunctionalities;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using FirstAPI.Models;

namespace ClinicManagement.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepository<string, User> _userRepository;
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly IRepository<int, Speciality> _specialityRepository;
    private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
    private readonly IOtherContextFunctionalities _otherContextFunctionalities;
    private readonly IMapper _mapper;
    private readonly IEncryptionService _encryptionService;
    private readonly DoctorMapper _doctorMapper;
    private readonly SpecialityMapper _specialityMapper;

    public DoctorService(IRepository<string, User> userRepository,
                         IRepository<int, Doctor> doctorRepository,
                         IRepository<int, Speciality> specialityRepository,
                         IRepository<int, DoctorSpeciality> doctorSpecialityRepository,
                         IOtherContextFunctionalities OtherContextFunctionalities,
                         IMapper mapper,
                         IEncryptionService encryptionService
                        )
    {
        _doctorMapper = new();
        _specialityMapper = new();
        _userRepository = userRepository;
        _doctorRepository = doctorRepository;
        _specialityRepository = specialityRepository;
        _doctorSpecialityRepository = doctorSpecialityRepository;
        _otherContextFunctionalities = OtherContextFunctionalities;
        _mapper = mapper;
        _encryptionService = encryptionService;
    }

    public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
    {
        try
        {
            var user = _mapper.Map<DoctorAddRequestDto, User>(doctor);
            
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel() { Data = doctor.Password });
            user.Password = encryptedData.EncryptedData;
            user.HashKey = encryptedData.HashKey;
            user.Role = "Doctor";

            user = await _userRepository.Create(user);

            var newDoctor = _doctorMapper.MapDoctorAddRequestDoctor(doctor);
            Console.WriteLine($"{user.UserName}, {newDoctor.Email}, {doctor.Password}, {user.Password}");

            newDoctor = await _doctorRepository.Create(newDoctor) ?? throw new Exception("Could not add doctor");
            if (doctor?.Specialities?.Count > 0)
            {
                int[] specialities = await MapAndAddSpeciality(doctor);
                for (int i = 0; i < specialities.Length; i++)
                {
                    var doctorSpeciality = _specialityMapper.MapDoctorSpeciality(newDoctor.Id, specialities[i]);
                    doctorSpeciality = await _doctorSpecialityRepository.Create(doctorSpeciality);
                }
            }
            return newDoctor;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    private async Task<int[]> MapAndAddSpeciality(DoctorAddRequestDto doctor)
    {
        int[] specialityIds = new int[doctor.Specialities?.Count ?? 0];
        IEnumerable<Speciality>? existingSpecialities = null;
        try
        {
            existingSpecialities = await _specialityRepository.GetAll();
        }
        catch (Exception e)
        {

        }
        int count = 0;
        foreach (var item in doctor.Specialities )
        {
            Speciality? speciality = null;
            if (existingSpecialities != null)
                speciality = existingSpecialities.FirstOrDefault(s => s.Name.ToLower() == item.Name.ToLower());
            if (speciality == null)
            {
                speciality = _specialityMapper.MapSpecilityAddRequestDoctor(item);
                speciality = await _specialityRepository.Create(speciality);
            }
            specialityIds[count] = speciality.Id;
            count++;
        }
        return specialityIds;
    }


    public Task<Doctor> GetDoctByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<DoctorsBySpecialityDto>> GetDoctorsBySpeciality(string speciality)
    {
        var doctors = await _otherContextFunctionalities.GetDoctorsBySpeciality(speciality) ?? throw new Exception("No doctors found for the given speciality");
        return doctors;
    }

    public async Task<Doctor> GetById(int id)
    {
        var doctor = await _doctorRepository.GetById(id);
        return doctor;
    }

}
