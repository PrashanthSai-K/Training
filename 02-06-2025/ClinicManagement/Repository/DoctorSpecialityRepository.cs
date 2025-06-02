using System;
using ClinicManagement.Context;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repository;

public class DoctorSpecialityRepository : Repository<int, DoctorSpeciality>
{
    public DoctorSpecialityRepository(ClinicDBContext clinicDBContext) : base(clinicDBContext)
    {
    }

    public override async Task<IEnumerable<DoctorSpeciality>> GetAll()
    {
        var specialities = await _clinicDBContext.DoctorSpecialities.ToListAsync();
        return (specialities == null  || specialities.Count == 0 ) ? throw new Exception("No Doctor Speciality found in db") : specialities;
    }

    public override async Task<DoctorSpeciality> GetById(int id)
    {
        var speciality = await _clinicDBContext.DoctorSpecialities.SingleOrDefaultAsync(p => p.Id == id);
        return speciality ?? throw new Exception($"Doctor Speciality With {id} not found");
    }
}
