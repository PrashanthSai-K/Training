using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
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

    public DbSet<DoctorsBySpecialityDto> DoctorsBySpeciality { get; set; }

    public DbSet<User> Users { get; set; }

    public async Task<List<DoctorsBySpecialityDto>> GetDoctorsBySpeciality(string speciality)
    {
        return await this.Set<DoctorsBySpecialityDto>()
                .FromSqlInterpolated($"SELECT * FROM proc_GetDoctorsBySpeciality({speciality})")
                .ToListAsync();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithOne(u => u.Patient)
            .HasForeignKey<Patient>(p => p.Email)
            .HasConstraintName("FK_User_Patient")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.User)
            .WithOne(u => u.Doctor)
            .HasForeignKey<Doctor>(d => d.Email)
            .HasConstraintName("FK_User_Doctor")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasKey(app => app.AppointmentId)
            .HasName("PK_Appointment_Id");

        modelBuilder.Entity<Appointment>()
            .HasOne(app => app.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(app => app.PatientId)
            .HasConstraintName("FK_Appointment_Patient")
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(app => app.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(app => app.DoctorId)
            .HasConstraintName("FK_Appointment_Doctor")
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<DoctorSpeciality>()
            .HasOne(ds => ds.Doctor)
            .WithMany(d => d.DoctorSpecialities)
            .HasForeignKey(ds => ds.DoctorId)
            .HasConstraintName("FK_DocSpeciality_Doctor")
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<DoctorSpeciality>()
            .HasOne(ds => ds.Speciality)
            .WithMany(s => s.DoctorSpecialities)
            .HasForeignKey(ds => ds.SpecialityId)
            .HasConstraintName("FK_DocSpeciality_Speciality")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
