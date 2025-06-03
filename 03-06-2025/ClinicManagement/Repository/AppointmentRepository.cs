using System;
using ClinicManagement.Context;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repository;

public class AppointmentRepository : Repository<int, Appointment>
{
    public AppointmentRepository(ClinicDBContext clinicDBContext) : base(clinicDBContext)
    {
    }

    public override async Task<IEnumerable<Appointment>> GetAll()
    {
        var appointments = await _clinicDBContext.Appointments.ToListAsync();
        return (appointments == null  || appointments.Count == 0 ) ? throw new Exception("No Appointments found in db") : appointments;
    }

    public override async Task<Appointment> GetById(int id)
    {
        var appointment = await _clinicDBContext.Appointments.SingleOrDefaultAsync(p => p.AppointmentId == id);
        return appointment ?? throw new Exception($"Appointment With {id} not found");
    }
}
