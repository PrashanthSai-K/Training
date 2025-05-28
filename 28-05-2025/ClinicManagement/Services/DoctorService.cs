using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;

namespace ClinicManagement.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepository<int, Doctor> _doctors;
    private readonly IRepository<int, DoctorSpeciality> _doctorSpecialities;
    private readonly ISpecialityService _specialityService;


    public DoctorService(IRepository<int, Doctor> repository, IRepository<int, DoctorSpeciality> doctorSpecialityRepo, ISpecialityService specialityService)
    {
        _doctors = repository;
        _specialityService = specialityService;
        _doctorSpecialities = doctorSpecialityRepo;
    }

    public async Task<Doctor> CreateDoctor(Doctor doctor)
    {
        await _doctors.Create(doctor);
        return doctor;
    }

    public async Task<Doctor> DeleteDoctor(int id)
    {
        var doctor = await _doctors.GetById(id);
        await _doctors.Delete(id);
        return doctor;
    }

    public async Task<Doctor> GetById(int id)
    {
        return await _doctors.GetById(id);
    }

    public async Task<IEnumerable<Doctor>> GetDoctors(DoctorSearch doctorSearch)
    {
        var doctors = await _doctors.GetAll();
        doctors = GetDoctorsByName(doctorSearch.Name, doctors);
        doctors = await GetDoctorsBySpeciality(doctorSearch.Speciality, doctors);
        return doctors;
    }

    public IEnumerable<Doctor> GetDoctorsByName(string? name, IEnumerable<Doctor> doctors)
    {
        if(name == null || name == string.Empty)
            return doctors;
        return doctors.Where(d => d.Name == name).ToList();
    }
    
    public async Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(string? speciality, IEnumerable<Doctor> doctors)
    {
        if (speciality == null || speciality == string.Empty)
            return doctors;

        var doctorSpecialities = await _doctorSpecialities.GetAll();
        var specialities = await _specialityService.GetAllSpecialities();

        var filteredSpecialityIds = specialities
            .Where(s => s.Name == speciality)
            .Select(s => s.Id);

        var matchingDoctorIds = doctorSpecialities
            .Where(ds => filteredSpecialityIds.Contains(ds.SpecialityId))
            .Select(ds => ds.DoctorId);

        var result = doctors
            .Where(d => matchingDoctorIds.Contains(d.Id));

        return result;
    }


    public async Task<Doctor> UpdateDoctor(Doctor doctor)
    {
        await _doctors.Update(doctor.Id, doctor);
        return doctor;
    }

    public async Task<Speciality> AddDoctorSpeciality(int doctorId, int specialityId)
    {
        var doctor = await _doctors.GetById(doctorId);
        var speciality = await _specialityService.GetSpecialityById(specialityId);

        var docSpeciality = new DoctorSpeciality();
        docSpeciality.DoctorId = doctorId;
        docSpeciality.SpecialityId = specialityId;

        await _doctorSpecialities.Create(docSpeciality);
        doctor?.DoctorSpecialities?.Add(docSpeciality);

        return speciality;
    }

    public async Task<Speciality> RemoveDoctorSpeciality(int doctorId, int specialityId)
    {
        var doctor = await _doctors.GetById(doctorId);
        var speciality = await _specialityService.GetSpecialityById(specialityId);

        var docSpecialities = await _doctorSpecialities.GetAll();
        var docSpeciality = docSpecialities.FirstOrDefault(ds => ds.DoctorId == doctorId && ds.SpecialityId == specialityId) ?? throw new Exception("Doctor with given speciality not found");
        await _doctorSpecialities.Delete(docSpeciality.Id);
        return speciality;
    }

    public async Task<IEnumerable<Speciality>> GetDoctorSpecialities(int doctorId)
    {
        var doctor = await _doctors.GetById(doctorId);
        var specialities = await _specialityService.GetAllSpecialities();

        var docSpecialities = await _doctorSpecialities.GetAll();

        return specialities
                .Where(s => docSpecialities.Where(ds => ds.DoctorId == doctorId).Select(ds => ds.SpecialityId).Contains(s.Id))
                .ToList() ?? throw new Exception("No Specialities found for doctor");
    }
}
