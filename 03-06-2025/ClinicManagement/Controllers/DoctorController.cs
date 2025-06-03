using System.Threading.Tasks;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor([FromBody] DoctorAddRequestDto doctorAddRequestDto)
        {
            var doctor = await _doctorService.AddDoctor(doctorAddRequestDto);
            return Ok(doctor);
        }

        [HttpGet]
        public async Task<ActionResult<DoctorsBySpecialityDto>> GetDoctorBySpeciality(string speciality)
        {
            var doctors = await _doctorService.GetDoctorsBySpeciality(speciality);
            return Ok(doctors);
        }
    }
}
