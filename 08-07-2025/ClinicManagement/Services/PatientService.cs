using System;
using System.Threading.Tasks;
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
    public async Task<Patient> CreatePatient(Patient patient)
    {
        await _patients.Create(patient);
        return patient;
    }

    public async Task<Patient> DeletePatient(int id)
    {
        var patient = await _patients.GetById(id);
        await _patients.Delete(id);
        return patient;
    }

    public async Task<Patient> GetById(int id)
    {
        return await _patients.GetById(id);
    }

    public async Task<IEnumerable<Patient>> GetPatients()
    {
        return await _patients.GetAll();
    }

    public async Task<Patient> UpdatePatient(Patient patient)
    {
        await _patients.Update(patient.Id, patient);
        return patient;
    }
}
