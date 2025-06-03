using System;

namespace ClinicManagement.Models.DTO;

public class DoctorsBySpecialityDto
{
    public int Id { get; set; }
    public string Dname { get; set; } = string.Empty;
    public float Yoe { get; set; }

}
