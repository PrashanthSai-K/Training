using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetPatients()
        {
            var patients = _patientService.GetPatients();
            return Ok(patients);
        }
        [HttpPost]
        public ActionResult<Patient> CreatePatients(Patient patient)
        {
            var CreatedPatient = _patientService.CreatePatient(patient);
            return Ok(CreatedPatient);
        }
        [HttpPut]
        public ActionResult<Patient> UpdatePatients(Patient patient)
        {
            var UpdatedPatient = _patientService.UpdatePatient(patient);
            return Ok(UpdatedPatient);
        }

        [HttpDelete]
        public ActionResult<Patient> DeletePatient(int id)
        {
            var DeletedPatient = _patientService.DeletePatient(id);
            return Ok(DeletedPatient);
        }
    }
}
