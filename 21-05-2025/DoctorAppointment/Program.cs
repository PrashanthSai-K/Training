using DoctorAppointment.Controllers;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Repository;
using DoctorAppointment.Services;

public class Appointment
{
    public static void Main(string[] args)
    {
        IRepository<int, PatitentAppointment> repository = new AppointmentRepository();
        IAppointmentService service = new AppointmentService(repository);
        AppointmentController controller = new AppointmentController(service);
        controller.Start();
    }
}