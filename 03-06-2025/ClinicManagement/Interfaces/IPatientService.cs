using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Interfaces;

public interface IPatientService
{
    Task<Patient> GetById(int id);
    Task<IEnumerable<Patient>> GetPatients();

    Task<Patient> CreatePatient(PatientAddRequestDto patient);
    Task<Patient> UpdatePatient(Patient patient);
    Task<Patient> DeletePatient(int id);
}
