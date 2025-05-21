using System;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;

namespace DoctorAppointment.Controllers;

public class AppointmentController
{
    private IAppointmentService _appointmentService;
    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public void Start()
    {
        bool exit = false;
        while (true)
        {
            int option;
            Console.WriteLine("\nPlese select an option: \n   1. Add Appointment\n   2. Search Appointment\n");
            Console.Write("Enter option : ");
            while (!int.TryParse(Console.ReadLine(), out option))
                Console.WriteLine("Select a valid number.");
            switch (option)
            {
                case 1:
                    AddAppointment();
                    break;
                case 2:
                    SearchAppointment();
                    break;
                default:
                    exit = true;
                    break;
            }
            if (exit)
                break;
        }
    }

    public void AddAppointment()
    {
        PatitentAppointment appointment = new PatitentAppointment();
        appointment.TakeAppointmentDetailsFromUser();
        int result = _appointmentService.Add(appointment);
        Console.WriteLine($"\nAdded appointment Id : {result}");
    }

    public void SearchAppointment()
    {
        List<PatitentAppointment> appointments = _appointmentService.Search(PrintSearchMenu());
        if (appointments == null || appointments.Count == 0)
        {
            Console.WriteLine("No appointments found for the given criteria");
            return;
        }
        Console.WriteLine(string.Join("\n", appointments));
    }


    private SearchModel PrintSearchMenu()
    {
        SearchModel searchModel = new SearchModel();
        int option;

        Console.WriteLine("Search by Patient Name? Type 1 for yes, 2 for no:");
        while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2))
        {
            Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
        }
        if (option == 1)
        {
            Console.Write("Enter Patient Name: ");
            searchModel.PatientName = Console.ReadLine() ?? string.Empty;
        }

        Console.WriteLine("Search by Appointment Date? Type 1 for yes, 2 for no:");
        while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2))
        {
            Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
        }
        if (option == 1)
        {
            Console.Write("Enter Appointment Date (yyyy-MM-dd): ");
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid date. Please enter a valid date (e.g., 2025-05-20): ");
            }
            searchModel.AppointmentDate = date;
        }

        Console.WriteLine("Search by Patient Age Range? Type 1 for yes, 2 for no:");
        while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2))
        {
            Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
        }
        if (option == 1)
        {
            searchModel.AgeRange = new Range<int>();

            Console.Write("Enter Minimum Age: ");
            int minAge;
            while (!int.TryParse(Console.ReadLine(), out minAge) || minAge < 0)
            {
                Console.WriteLine("Invalid age. Please enter a valid minimum age:");
            }
            searchModel.AgeRange.MinVal = minAge;

            Console.Write("Enter Maximum Age: ");
            int maxAge;
            while (!int.TryParse(Console.ReadLine(), out maxAge) || maxAge < minAge)
            {
                Console.WriteLine($"Invalid age. Please enter a valid maximum age (>= {minAge}):");
            }
            searchModel.AgeRange.MaxVal = maxAge;
        }

        return searchModel;
    }

}
