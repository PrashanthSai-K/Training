using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicManagement.Models;

public class DoctorSpeciality
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int SpecialityId { get; set; }
    [JsonIgnore]
    public Doctor? Doctor { get; set; }
    [JsonIgnore]
    public Speciality? Speciality { get; set; }
}
