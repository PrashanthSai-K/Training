using System;
using ClinicManagement.Models;

namespace ClinicManagement.Interfaces;

public interface IDoctorService
{
    Doctor GetById(int id);
    ICollection<Doctor> GetDoctors();

    Doctor CreateDoctor(Doctor doctor);
    Doctor UpdateDoctor(Doctor doctor);
    Doctor DeleteDoctor(int id);
}
