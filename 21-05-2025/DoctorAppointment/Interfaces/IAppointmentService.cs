using System;
using DoctorAppointment.Models;

namespace DoctorAppointment.Interfaces;

public interface IAppointmentService
{
    int Add(PatitentAppointment patitentAppointment);
    List<PatitentAppointment> Search(SearchModel searchModel);
}
