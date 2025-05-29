using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Mappers;

public class SpecialityMapper
{
    public Speciality MapSpecilityAddRequestDoctor(SpecialityAddRequestDto addRequestDto)
    {
        Speciality speciality = new()
        {
            Name = addRequestDto.Name
        };
        return speciality;
    }

    public DoctorSpeciality MapDoctorSpeciality(int doctorId, int specialityId)
    {
        DoctorSpeciality doctorSpeciality = new()
        {
            DoctorId = doctorId,
            SpecialityId = specialityId
        };
        return doctorSpeciality;
    }
}
