using System;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;

namespace ClinicManagement.Services;

public class SpecialityService : ISpecialityService
{
    private readonly IRepository<int, Speciality> _specialities;

    public SpecialityService(IRepository<int, Speciality> repository)
    {
        _specialities = repository;
    }

    public async Task<Speciality> CreateSpeciality(Speciality speciality)
    {
        await _specialities.Create(speciality);
        return speciality;
    }

    public async Task<Speciality> DeleteSpeciality(int id)
    {
        var speciality = await _specialities.GetById(id);
        await _specialities.Delete(id);
        return speciality;
    }

    public Task<IEnumerable<Speciality>> GetAllSpecialities()
    {
        return _specialities.GetAll();
    }

    public async Task<Speciality> GetSpecialityById(int id)
    {
        return await _specialities.GetById(id);
    }
}
