using System;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Interfaces;

public interface IOtherContextFunctionalities
{
    Task<ICollection<DoctorsBySpecialityDto>> GetDoctorsBySpeciality(string speciality);
}
