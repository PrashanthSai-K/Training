using System;
using System.Text.Json.Serialization;

namespace ClinicManagement.Models;

public class Speciality
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    [JsonIgnore]
    public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }
}
