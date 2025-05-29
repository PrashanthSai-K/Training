using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ClinicManagement.Interfaces;
using ClinicManagement.Mappers;
using ClinicManagement.Misc.OtherFunctionalities;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly IRepository<int, Speciality> _specialityRepository;
    private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
    private readonly IOtherContextFunctionalities _otherContextFunctionalities;
    private readonly DoctorMapper _doctorMapper;
    private readonly SpecialityMapper _specialityMapper;

    public DoctorService(IRepository<int, Doctor> doctorRepository,
                         IRepository<int, Speciality> specialityRepository,
                         IRepository<int, DoctorSpeciality> doctorSpecialityRepository,
                         IOtherContextFunctionalities OtherContextFunctionalities
                        )
    {
        _doctorMapper = new();
        _specialityMapper = new();
        _doctorRepository = doctorRepository;
        _specialityRepository = specialityRepository;
        _doctorSpecialityRepository = doctorSpecialityRepository;
        _otherContextFunctionalities = OtherContextFunctionalities;
    }

    public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
    {
        try
        {
            var newDoctor = _doctorMapper.MapDoctorAddRequestDoctor(doctor);
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
        foreach (var item in doctor.Specialities)
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
    
}
