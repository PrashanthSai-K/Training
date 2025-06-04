using System;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Misc.OtherFunctionalities;

public class OtherContextFunctionalities : IOtherContextFunctionalities
{
    private readonly ClinicDBContext _clinicDBContext;

    public OtherContextFunctionalities(ClinicDBContext clinicDBContext)
    {
        _clinicDBContext = clinicDBContext;
    }
    public virtual async  Task<ICollection<DoctorsBySpecialityDto>> GetDoctorsBySpeciality(string speciality)
    {
        return await _clinicDBContext.GetDoctorsBySpeciality(speciality);
    }
}
