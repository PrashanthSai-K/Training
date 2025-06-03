using System;
using ClinicManagement.Context;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repository;

public class SpecialityRepository : Repository<int, Speciality>
{
    public SpecialityRepository(ClinicDBContext clinicDBContext) : base(clinicDBContext)
    {
    }
    public override async Task<IEnumerable<Speciality>> GetAll()
    {
        var specialities = await _clinicDBContext.Specialities.ToListAsync();
        return (specialities == null || specialities.Count == 0) ? throw new Exception("No Speciality found in db") : specialities;
    }

    public override async Task<Speciality> GetById(int id)
    {
        var speciality = await _clinicDBContext.Specialities.SingleOrDefaultAsync(p => p.Id == id);
        return speciality ?? throw new Exception($"Speciality With {id} not found");
    }
}
