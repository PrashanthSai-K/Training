using FirstApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        static List<Patient> patients = new List<Patient>
        {
            new Patient(101, "Hari", 18, "Fever")
        };

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetPatients()
        {
            return Ok(patients);
        }
        [HttpPost]
        public ActionResult<Patient> CreatePatients(Patient patient)
        {
            patients.Add(patient);
            return Created("", patient);
        }
        [HttpPut]
        public ActionResult<Patient> UpdatePatients(Patient patient)
        {
            var OldPatient = patients.FirstOrDefault(p => p.Id == patient.Id);
            if (OldPatient == null)
            {
                return NotFound("Patient not found");
            }
            OldPatient.Name = patient.Name;
            OldPatient.Age = patient.Age;
            OldPatient.Diagnosis = patient.Diagnosis;
            return Ok(OldPatient);
        }

        [HttpDelete]
        public ActionResult<Patient> DeletePatient(int id)
        {
            var OldPatient = patients.FirstOrDefault(p => p.Id == id);
            if (OldPatient == null)
            {
                return NotFound("Patient not found");
            }
            patients.Remove(OldPatient);
            return Ok(OldPatient);
        }
    }
}
