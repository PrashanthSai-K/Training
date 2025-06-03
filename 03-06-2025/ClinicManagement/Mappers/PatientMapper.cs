using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Mappers;

public class PatientMapper
{
    public Patient MapPatientAddRequestPatient(PatientAddRequestDto addRequestDto)
    {
        Patient patient = new()
        {
            Name = addRequestDto.Name,
            Email = addRequestDto.Email,
            Age = addRequestDto.Age,
            Phone = addRequestDto.Phone
        };
        return patient;
    }
}
