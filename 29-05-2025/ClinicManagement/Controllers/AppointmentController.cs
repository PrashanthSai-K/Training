using System.Threading.Tasks;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointments = await _appointmentService.GetAppointments();
            return Ok(appointments);
        }
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
        {
            var createdAppointment = await _appointmentService.CreateAppointment(appointment);
            return Created("", createdAppointment);
        }
        [HttpPut]
        public async Task<ActionResult<Appointment>> RescheduleAppointment(Appointment appointment)
        {
            var RescheduledAppointment = await _appointmentService.RescheduleAppointment(appointment);
            return Ok(RescheduledAppointment);
        }
        [HttpDelete]
        public async Task<ActionResult<Appointment>> CancelAppointment(int id)
        {
            var CancelledAppointment = await _appointmentService.CancelAppointment(id);
            return Ok(CancelledAppointment);
        }
    }

}
