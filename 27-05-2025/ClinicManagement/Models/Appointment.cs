using System;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagement.Services;

namespace ClinicManagement.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    [ForeignKey("PatientId")]
    public Patient? Patient { get; set; }
    [ForeignKey("DoctorId")]
    public Doctor? Doctor { get; set; }
}
