using System;
using System.Text.Json.Serialization;

namespace ClinicManagement.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public float YearsOfExperience { get; set; }
    [JsonIgnore]
    public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }
    [JsonIgnore]
    public ICollection<Appointment>? Appointments { get; set; }
}
