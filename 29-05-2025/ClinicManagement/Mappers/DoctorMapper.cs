using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Mappers;

public class DoctorMapper
{
    public Doctor MapDoctorAddRequestDoctor(DoctorAddRequestDto addRequestDto)
    {
        Doctor doctor = new()
        {
            Name = addRequestDto.Name,
            YearsOfExperience = addRequestDto.YearsOfExperience
        };
        return doctor;
    }
}
