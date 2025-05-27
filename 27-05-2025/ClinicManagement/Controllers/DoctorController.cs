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
        public ActionResult<IEnumerable<Doctor>> GetDoctors()
        {
            var doctors = _doctorService.GetDoctors();
            return Ok(doctors);
        }

        [HttpPost]
        public ActionResult<Doctor> CreateDoctor(Doctor doctor)
        {
            var CreatedDoctor = _doctorService.CreateDoctor(doctor);
            return Created("", CreatedDoctor);
        }
        [HttpPut]
        public ActionResult<Doctor> UpdateDoctor(Doctor doctor)
        {
           var UpdatedDoctor = _doctorService.UpdateDoctor(doctor);
            return Ok(UpdatedDoctor);
        }
        [HttpDelete]
        public ActionResult<Doctor> DeleteDoctor(int id)
        {
            var DeletedDoctor = _doctorService.DeleteDoctor(id);
            return Ok(DeletedDoctor);
        }
    }
}
