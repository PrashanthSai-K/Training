using System;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Mappers;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class DoctorServiceWithTransaction : IDoctorService
{
    private readonly ClinicDBContext _clinicDBContext;
    private readonly DoctorMapper _doctorMapper;
    private readonly SpecialityMapper _specialityMapper;


    public DoctorServiceWithTransaction(ClinicDBContext clinicDBContext)
    {
        _doctorMapper = new();
        _specialityMapper = new();
        _clinicDBContext = clinicDBContext;
    }

    public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
    {
        using var transaction = await _clinicDBContext.Database.BeginTransactionAsync();
        var newDoctor = _doctorMapper.MapDoctorAddRequestDoctor(doctor);
        try
        {
            await _clinicDBContext.AddAsync(newDoctor);
            await _clinicDBContext.SaveChangesAsync();

            if (doctor?.Specialities?.Count > 0)
            {
                var existingSpecialities = await _clinicDBContext.Specialities.ToListAsync();
                foreach (var item in doctor.Specialities)
                {
                    var speciality = await _clinicDBContext.Specialities.FirstOrDefaultAsync(s => s.Name.ToLower() == item.Name.ToLower());
                    if (speciality != null)
                    {
                        speciality = _specialityMapper.MapSpecilityAddRequestDoctor(item);
                        await _clinicDBContext.AddAsync(speciality);
                        await _clinicDBContext.SaveChangesAsync();
                    }
                    var doctorSpeciality = _specialityMapper.MapDoctorSpeciality(newDoctor.Id, speciality.Id);
                    await _clinicDBContext.AddAsync(doctorSpeciality);
                }
                await _clinicDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return newDoctor;
            }
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        return null;
    }

    public Task<Doctor> GetDoctByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<DoctorsBySpecialityDto>> GetDoctorsBySpeciality(string speciality)
    {
        throw new NotImplementedException();
    }
}
