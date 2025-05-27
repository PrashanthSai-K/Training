using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models;

public class DoctorSpeciality
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int SpecialityId { get; set; }
    [ForeignKey("DoctorId")]
    public Doctor? Doctor { get; set; }
    [ForeignKey("SpecialityId")]
    public Speciality? Speciality { get; set; }
}
