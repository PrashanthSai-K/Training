using System;
using ClinicManagement.Models;

namespace ClinicManagement.Interfaces;

public interface IPatientService
{
    Patient GetById(int id);
    ICollection<Patient> GetPatients();

    Patient CreatePatient(Patient patient);
    Patient UpdatePatient(Patient patient);
    Patient DeletePatient(int id);
}
