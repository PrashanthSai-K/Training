using System;
using ClinicManagement.Models;

namespace ClinicManagement.Interfaces;

public interface IDoctorService
{
    Task<Doctor> GetById(int id);
    Task<IEnumerable<Doctor>> GetDoctors(DoctorSearch doctorSearch);

    // Task<IEnumerable<Doctor>> SearchDoctors();

    Task<Doctor> CreateDoctor(Doctor doctor);
    Task<Doctor> UpdateDoctor(Doctor doctor);
    Task<Doctor> DeleteDoctor(int id);

    Task<Speciality> AddDoctorSpeciality(int DoctorId, int SpecialityId);
    Task<Speciality> RemoveDoctorSpeciality(int DoctorId, int SpecialityId);

    Task<IEnumerable<Speciality>> GetDoctorSpecialities(int DoctorId);

}
