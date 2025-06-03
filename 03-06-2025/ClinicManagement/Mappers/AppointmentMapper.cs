using System;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;

namespace ClinicManagement.Mappers;

public class AppointmentMapper
{
    public Appointment MapAppointmentAddRequestAppointment(AppointmentAddRequestDto addRequestDto)
    {
        Appointment appointment = new()
        {
            PatientId = addRequestDto.PatientId,
            DoctorId = addRequestDto.DoctorId,
            AppointmentDate = addRequestDto.AppointmentDate
        };
        return appointment;
    }

}
