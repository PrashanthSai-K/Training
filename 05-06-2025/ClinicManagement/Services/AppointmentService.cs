using System;
using System.Threading.Tasks;
using ClinicManagement.Interfaces;
using ClinicManagement.Mappers;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<int, Appointment> _appointments;
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly AppointmentMapper _appointmentMapper;

    public AppointmentService(IRepository<int, Appointment> repository, IDoctorService doctorService, IPatientService patientService)
    {
        _appointments = repository;
        _doctorService = doctorService;
        _patientService = patientService;
        _appointmentMapper = new();
    }
    public async Task<Appointment> CancelAppointment(int id)
    {
        var OldAppointment = await _appointments.GetById(id);
        await _appointments.Delete(id);
        return OldAppointment;
    }

    public async Task<Appointment> CreateAppointment(AppointmentAddRequestDto appointment)
    {
        await _doctorService.GetById(appointment.DoctorId);
        await _patientService.GetById(appointment.PatientId);
        if (appointment.AppointmentDate < DateTime.Now)
        {
            throw new Exception("Appointment Date must be in future");
        }

        var newappointment = _appointmentMapper.MapAppointmentAddRequestAppointment(appointment);
        newappointment.Status = "Booked";

        await _appointments.Create(newappointment);
        return newappointment;
    }

    public async Task<Appointment> GetAppointmentById(int id)
    {
        return await _appointments.GetById(id);
    }

    public async Task<IEnumerable<Appointment>> GetAppointments()
    {
        return await _appointments.GetAll();
    }

    public async Task<Appointment> RescheduleAppointment(Appointment appointment)
    {
        var OldAppointment = await _appointments.GetById(appointment.AppointmentId);
        await _doctorService.GetById(appointment.DoctorId);
        await _patientService.GetById(appointment.PatientId);

        if (OldAppointment.AppointmentDate > DateTime.Now)
            throw new Exception("Future Appointment cannot be rescheduled");

        if (appointment.AppointmentDate < DateTime.Now)
            throw new Exception("Appointment Date must be in future");

        await _appointments.Update(appointment.AppointmentId, appointment);
        return appointment;
    }
}
