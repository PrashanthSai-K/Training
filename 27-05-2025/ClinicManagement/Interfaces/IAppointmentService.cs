using System;
using ClinicManagement.Models;

namespace ClinicManagement.Interfaces;

public interface IAppointmentService
{
    Appointment GetAppointmentById(int id);
    ICollection<Appointment> GetAppointments();
    Appointment CreateAppointment(Appointment appointment);
    Appointment RescheduleAppointment(Appointment appointment);
    Appointment CancelAppointment(int id);
}
