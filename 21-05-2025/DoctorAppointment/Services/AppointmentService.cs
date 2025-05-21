using System;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;

namespace DoctorAppointment.Services;

public class AppointmentService : IAppointmentService
{
    private IRepository<int, PatitentAppointment> _repository;
    public AppointmentService(IRepository<int, PatitentAppointment> repository)
    {
        _repository = repository;
    }
    public int Add(PatitentAppointment patitentAppointment)
    {
        var result = _repository.Add(patitentAppointment);
        if (result != null)
            return result.Id;
        return -1;
    }

    public List<PatitentAppointment>? Search(SearchModel searchModel)
    {
        var appointments = _repository.GetAll();
        appointments = SearchByName(appointments, searchModel.PatientName);
        appointments = SearchByAppointmentDate(appointments, searchModel.AppointmentDate);
        appointments = SearchByAge(appointments, searchModel.AgeRange);
        if (appointments.Count > 0)
            return appointments.ToList();
        return null;
    }

    public ICollection<PatitentAppointment>? SearchByName(ICollection<PatitentAppointment> appointments, string name)
    {
        if (appointments == null || name == null)
            return appointments;
        return appointments.Where(a => a.PatientName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public ICollection<PatitentAppointment>? SearchByAppointmentDate(ICollection<PatitentAppointment> appointments, DateTime? date)
    {
        if (date == null || appointments == null || appointments.Count == 0)
            return appointments;
        return appointments.Where(a => a.AppointmentDate == date).ToList();
    }

    public ICollection<PatitentAppointment>? SearchByAge(ICollection<PatitentAppointment> appointments, Range<int> Age)
    {
        if (Age == null || appointments == null || appointments.Count == 0)
            return appointments;
        return appointments.Where(a => a.PatientAge >= Age.MinVal && a.PatientAge <= Age.MaxVal).ToList();
    }

}
