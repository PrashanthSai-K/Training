using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Interfaces;

public interface IDoctorService
{
    public Task<Doctor> GetDoctByName(string name);
    public Task<ICollection<DoctorsBySpecialityDto>> GetDoctorsBySpeciality(string speciality);
    public Task<Doctor> AddDoctor(DoctorAddRequestDto doctor);
    public Task<Doctor> GetById(int id);

}
