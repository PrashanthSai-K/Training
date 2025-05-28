using System.Threading.Tasks;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors([FromQuery] DoctorSearch doctorSearch)
        {
            var doctors = await _doctorService.GetDoctors(doctorSearch);
            return Ok(doctors);
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
        {
            var CreatedDoctor = await _doctorService.CreateDoctor(doctor);
            return Created("", CreatedDoctor);
        }
        [HttpPut]
        public async Task<ActionResult<Doctor>> UpdateDoctor(Doctor doctor)
        {
            var UpdatedDoctor = await _doctorService.UpdateDoctor(doctor);
            return Ok(UpdatedDoctor);
        }
        [HttpDelete]
        public async Task<ActionResult<Doctor>> DeleteDoctor(int id)
        {
            var DeletedDoctor = await _doctorService.DeleteDoctor(id);
            return Ok(DeletedDoctor);
        }

        [HttpGet]
        [Route("GetDoctorSpeciality")]
        public async Task<ActionResult<IEnumerable<Speciality>>> GetDoctorSpecialities(int DoctorId)
        {
            var specialities = await _doctorService.GetDoctorSpecialities(DoctorId);
            return Ok(specialities);
        }

        [HttpPost]
        [Route("AddSpeciality")]
        public async Task<ActionResult> AddDoctorSpeciality(int DoctorId, int SpecialityId)
        {
            var speciality = await _doctorService.AddDoctorSpeciality(DoctorId, SpecialityId);
            return Ok($"{speciality.Name} added to doctor");
        }

        [HttpDelete]
        [Route("RemoveSpeciality")]
        public async Task<ActionResult> RemoveDoctorSpeciality(int DoctorId, int SpecialityId)
        {
            var speciality = await _doctorService.RemoveDoctorSpeciality(DoctorId, SpecialityId);
            return Ok($"{speciality.Name} removed from doctor");
        }
    }
}
