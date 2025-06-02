using System.Threading.Tasks;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            var patients = await _patientService.GetPatients();
            return Ok(patients);
        }
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatients(PatientAddRequestDto patient)
        {
            var CreatedPatient = await _patientService.CreatePatient(patient);
            return Ok(CreatedPatient);
        }
        [HttpPut]
        public async Task<ActionResult<Patient>> UpdatePatients(Patient patient)
        {
            var UpdatedPatient = await _patientService.UpdatePatient(patient);
            return Ok(UpdatedPatient);
        }

        [HttpDelete]
        public async Task<ActionResult<Patient>> DeletePatient(int id)
        {
            var DeletedPatient = await _patientService.DeletePatient(id);
            return Ok(DeletedPatient);
        }
    }
}
