using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Interfaces;

public interface IAppointmentService
{
    Task<Appointment> GetAppointmentById(int id);
    Task<IEnumerable<Appointment>> GetAppointments();
    Task<Appointment> CreateAppointment(AppointmentAddRequestDto appointment);
    Task<Appointment> RescheduleAppointment(Appointment appointment);
    Task<Appointment> CancelAppointment(int id);
}
