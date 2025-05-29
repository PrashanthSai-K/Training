using System;
using ClinicManagement.Models;

namespace ClinicManagement.Interfaces;

public interface IPatientService
{
    Task<Patient> GetById(int id);
    Task<IEnumerable<Patient>> GetPatients();

    Task<Patient> CreatePatient(Patient patient);
    Task<Patient> UpdatePatient(Patient patient);
    Task<Patient> DeletePatient(int id);
}
