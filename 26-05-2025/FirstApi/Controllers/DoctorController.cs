using FirstApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        static List<Doctor> doctors = new List<Doctor>
        {
            new Doctor(101,"Sai")
        };
        [HttpGet]
        public ActionResult<IEnumerable<Doctor>> GetDoctors()
        {
            return Ok(doctors);
        }

        [HttpPost]
        public ActionResult<Doctor> CreateDoctor(Doctor doctor)
        {
            doctors.Add(doctor);
            return Created("", doctor);
        }
        [HttpPut]
        public ActionResult<Doctor> UpdateDoctor(Doctor doctor)
        {
            var oldDoctor = doctors.FirstOrDefault((d)=>d.Id == doctor.Id);

            if (oldDoctor == null)
            {
                return NotFound("Doctor not found");
            }
            oldDoctor.Name = doctor.Name;
            return Ok(doctor);
        }
        [HttpDelete]
        public ActionResult<Doctor> DeleteDoctor(int id)
        {
            var oldDoctor = doctors.FirstOrDefault((d) => d.Id == id);

            if (oldDoctor == null)
            {
                return NotFound("Doctor not found");
            }
            doctors.Remove(oldDoctor);
            return Ok(oldDoctor);
        }
    }
}
