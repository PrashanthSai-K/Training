using System;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;

namespace ClinicManagement.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<int, Appointment> _appointments;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;

    public AppointmentService(IRepository<int, Appointment> repository, IDoctorService doctorService, IPatientService patientService)
    {
        _appointments = repository;
        _doctorService = doctorService;
        _patientService = patientService;
    }
    public Appointment CancelAppointment(int id)
    {
        var OldAppointment = _appointments.GetById(id);
        _appointments.Delete(id);
        return OldAppointment;
    }

    public Appointment CreateAppointment(Appointment appointment)
    {
        _doctorService.GetById(appointment.DoctorId);
        _patientService.GetById(appointment.PatientId);
        if (appointment.AppointmentDate < DateTime.Now)
        {
            throw new Exception("Appointment Date must be in future");
        }
        _appointments.Create(appointment);
        return appointment;  
    }

    public Appointment GetAppointmentById(int id)
    {
       return  _appointments.GetById(id);
    }

    public ICollection<Appointment> GetAppointments()
    {
        return _appointments.GetAll();
    }

    public Appointment RescheduleAppointment(Appointment appointment)
    {
        var OldAppointment = _appointments.GetById(appointment.Id);
        _doctorService.GetById(appointment.DoctorId);
        _patientService.GetById(appointment.PatientId);
        if (appointment.AppointmentDate < DateTime.Now)
        {
            throw new Exception("Appointment Date must be in future");
        }
        _appointments.Update(appointment);
        return appointment;
    }
}
