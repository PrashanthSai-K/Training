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
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            var appointments = _appointmentService.GetAppointments();
            return Ok(appointments);
        }
        [HttpPost]
        public ActionResult<Appointment> CreateAppointment(Appointment appointment)
        {
            var createdAppointment = _appointmentService.CreateAppointment(appointment);
            return Created("", createdAppointment);
        }
        [HttpPut]
        public ActionResult<Appointment> RescheduleAppointment(Appointment appointment)
        {
            var RescheduledAppointment = _appointmentService.RescheduleAppointment(appointment);
            return Ok(RescheduledAppointment);
        }
        [HttpDelete]
        public ActionResult<Appointment> CancelAppointment(int id)
        {
            var CancelledAppointment = _appointmentService.CancelAppointment(id);
            return Ok(CancelledAppointment);
        }
    }

}
