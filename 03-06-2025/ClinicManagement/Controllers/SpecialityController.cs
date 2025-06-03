using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialityController : ControllerBase
    {
        private readonly ISpecialityService _specialityService;

        public SpecialityController(ISpecialityService specialityService)
        {
            _specialityService = specialityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Speciality>>> GetSpecialities()
        {
            var specialities = await _specialityService.GetAllSpecialities();
            return Ok(specialities);
        }

        [HttpPost]
        public async Task<ActionResult<Speciality>> CreateSpeciality(Speciality speciality)
        {
            await _specialityService.CreateSpeciality(speciality);
            return Created("", speciality);
        }

        [HttpDelete]
        public async Task<ActionResult<Speciality>> DeleteSpeciality(int id)
        {
            var speciality = await _specialityService.DeleteSpeciality(id);
            return Ok(speciality);
        }

    }
}
