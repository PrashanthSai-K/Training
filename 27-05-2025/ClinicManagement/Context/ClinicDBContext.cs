using System;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Context;

public class ClinicDBContext : DbContext
{
    public ClinicDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Speciality> Specialities { get; set; }
    public DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }
    
}
