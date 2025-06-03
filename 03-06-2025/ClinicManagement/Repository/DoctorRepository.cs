using System;
using ClinicManagement.Context;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repository;

public class DoctorRepository : Repository<int, Doctor>
{
    public DoctorRepository(ClinicDBContext clinicDBContext) : base(clinicDBContext)
    {

    }
    public override async Task<IEnumerable<Doctor>> GetAll()
    {
        var doctors = await _clinicDBContext.Doctors.ToListAsync();
        return (doctors == null  || doctors.Count == 0 ) ? throw new Exception("No Doctors found in db") : doctors;
    }

    public override async Task<Doctor> GetById(int id)
    {
        var doctor = await _clinicDBContext.Doctors.SingleOrDefaultAsync(p => p.Id == id);
        return doctor ?? throw new Exception($"Doctor With {id} not found");
    }
}
