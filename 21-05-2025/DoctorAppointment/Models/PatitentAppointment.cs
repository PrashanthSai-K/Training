using System;

namespace DoctorAppointment.Models;

public class PatitentAppointment
{
    public int Id { get; set; }
    public string PatientName { get; set; }
    public int PatientAge { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; }

    public PatitentAppointment()
    {
        PatientName = string.Empty;
        Reason = string.Empty;
    }

    public override string ToString()
    {
        return $"\nPatient Name : {PatientName}\nPatient Age : {PatientAge}\nAppointment Date : {AppointmentDate}\nReason : {Reason}";
    }

    public void TakeAppointmentDetailsFromUser()
    {
        Console.Write("Enter Patient Name: ");
        PatientName = Console.ReadLine() ?? string.Empty;

        int age;
        Console.Write("Enter Patient Age: ");
        while (!int.TryParse(Console.ReadLine(), out age) || age < 0)
        {
            Console.Write("Invalid input. Enter a valid age: ");
        }
        PatientAge = age;

        DateTime date;
        Console.Write("Enter Appointment Date (yyyy-MM-dd): ");
        while (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.Write("Invalid date. Please enter again (yyyy-MM-dd): ");
        }
        AppointmentDate = date;

        Console.Write("Enter Reason for Appointment: ");
        Reason = Console.ReadLine() ?? string.Empty;
    }

}
