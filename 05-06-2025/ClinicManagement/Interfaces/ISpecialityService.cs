using System;
using ClinicManagement.Models;

namespace ClinicManagement.Interfaces;

public interface ISpecialityService
{
    Task<Speciality> CreateSpeciality(Speciality speciality);
    Task<Speciality> DeleteSpeciality(int id);
    Task<Speciality> GetSpecialityById(int id);
    Task<IEnumerable<Speciality>> GetAllSpecialities();
}
