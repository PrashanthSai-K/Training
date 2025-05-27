using System;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;

namespace ClinicManagement.Services;

public class PatientService : IPatientService
{
    private readonly IRepository<int, Patient> _patients;

    public PatientService(IRepository<int, Patient> repository)
    {
        _patients = repository;
    }
    public Patient CreatePatient(Patient patient)
    {
        _patients.Create(patient);
        return patient;
    }

    public Patient DeletePatient(int id)
    {
        var patient = _patients.GetById(id);
        _patients.Delete(id);
        return patient;
    }

    public Patient GetById(int id)
    {
        return _patients.GetById(id);
    }

    public ICollection<Patient> GetPatients()
    {
        return _patients.GetAll();
    }

    public Patient UpdatePatient(Patient patient)
    {
        var OldPatient = _patients.GetById(patient.Id);
        _patients.Update(patient);
        return patient;
    }
}
