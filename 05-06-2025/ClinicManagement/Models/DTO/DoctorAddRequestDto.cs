using System;
using ClinicManagement.Misc;

namespace ClinicManagement.Models.DTO;

public class DoctorAddRequestDto
{
        [NameValidation(ErrorMessage = "Name must be a valid string")]
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<SpecialityAddRequestDto>? Specialities { get; set; }
        public float YearsOfExperience { get; set; }
        public string Password { get; set; } = string.Empty;

}

