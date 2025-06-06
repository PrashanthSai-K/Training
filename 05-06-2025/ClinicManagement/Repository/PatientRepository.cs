using System;
using ClinicManagement.Context;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repository;

public class PatientRepository : Repository<int, Patient>
{
    public PatientRepository(ClinicDBContext clinicDBContext) : base(clinicDBContext)
    {
    }

    public override async Task<IEnumerable<Patient>> GetAll()
    {
        var patients = await _clinicDBContext.Patients.ToListAsync();
        return (patients == null  || patients.Count == 0 ) ? throw new Exception("No patients found in db") : patients;
    }

    public override async Task<Patient> GetById(int id)
    {
        var patient = await _clinicDBContext.Patients.FirstOrDefaultAsync(p => p.Id == id);
        return patient ?? throw new Exception($"Patient With {id} not found");
    }
}
