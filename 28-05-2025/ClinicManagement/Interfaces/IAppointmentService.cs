using System;
using ClinicManagement.Models;

namespace ClinicManagement.Interfaces;

public interface IAppointmentService
{
    Task<Appointment> GetAppointmentById(int id);
    Task<IEnumerable<Appointment>> GetAppointments();
    Task<Appointment> CreateAppointment(Appointment appointment);
    Task<Appointment> RescheduleAppointment(Appointment appointment);
    Task<Appointment> CancelAppointment(int id);
}
