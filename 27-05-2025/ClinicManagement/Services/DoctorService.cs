using System;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;

namespace ClinicManagement.Services;

public class DoctorService : IDoctorService
{
    private readonly IRepository<int, Doctor> _doctors;

    public DoctorService(IRepository<int, Doctor> repository)
    {
        _doctors= repository;
    }

    public Doctor CreateDoctor(Doctor doctor)
    {
        _doctors.Create(doctor);
        return doctor;
    }

    public Doctor DeleteDoctor(int id)
    {
        var doctor = _doctors.GetById(id);
        _doctors.Delete(id);
        return doctor;
    }

    public Doctor GetById(int id)
    {
        return _doctors.GetById(id);
    }

    public ICollection<Doctor> GetDoctors()
    {
        return _doctors.GetAll();
    }

    public Doctor UpdateDoctor(Doctor doctor)
    {
        var Olddoctor = _doctors.GetById(doctor.Id);
        _doctors.Update(doctor);
        return Olddoctor;
    }
}
