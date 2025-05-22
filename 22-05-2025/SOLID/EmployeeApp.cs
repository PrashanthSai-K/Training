using System;
using Delegates;
using SOLID.Interfaces;
using SOLID.Models;

namespace SOLID;

public class EmployeeApp
{
    readonly IEmployeeService _employeeService;
    public EmployeeApp(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public void Start()
    {
        bool stop = false;
        while (true)
        {
            int option;
            Console.WriteLine("\nPlese select an option: \n   1. Add Employee\n   2. Search Employee\n   3. Stop\n");
            Console.Write("Enter option : ");
            while (!int.TryParse(Console.ReadLine(), out option))
                Console.WriteLine("Select a valid number.");
            switch (option)
            {
                case 1:
                    Console.WriteLine($"Added Employee Id : {AddEmployee()}");
                    break;
                case 2:
                    List<Employee>? employees = SearchEmployees();
                    if (employees == null)
                        Console.WriteLine("\nNo employees found for the given criteria");
                    else
                        Console.WriteLine(string.Join("\n", employees));
                    break;
                case 3:
                    stop = true;
                    break;
                default:
                    break;
            }
            if (stop)
                break;
        }
    }
    public int AddEmployee()
    {
        Employee employee = new EmployeeInputHandler().TakeEmployeeDetailsFromUser();
        return _employeeService.AddEmployee(employee);
    }

    public List<Employee> SearchEmployees()
    {
        SearchModel searchModel = new SearchModel();
        int id = GetInputNumber("Enter Id to search (Press Enter if not required): ");
        searchModel.Id = id == -1 ? null : id;

        searchModel.Name = GetInputString("Enter name to search (Press Enter if not required): ");

        double minSalary = GetInputDouble("Enter Minimum Salary to Search (Press Enter if not required): ");
        double maxSalary = GetInputDouble("Enter Maximum Salary to Search (Press Enter if not required): ");
        searchModel.Salary = new Range<double>();
        searchModel.Salary.MinVal = minSalary == -1 ? double.MinValue : minSalary;
        searchModel.Salary.MaxVal = maxSalary == -1 ? double.MaxValue : maxSalary;

        int minAge = GetInputNumber("Enter Min Age to search (Press Enter if not required): ");
        int maxAge = GetInputNumber("Enter Max Age to search (Press Enter if not required): ");
        searchModel.Age = new Range<int>();
        searchModel.Age.MinVal = minAge == -1 ? int.MinValue : minAge;

        searchModel.Age.MaxVal = maxAge == -1 ? int.MaxValue : maxAge;

        return _employeeService.SearchEmployee(searchModel);
    }

    public static int GetInputNumber(string promt)
    {
        int num;
        Console.Write(promt);
        while (!int.TryParse(Console.ReadLine(), out num))
            return -1;

        return num;
    }

    public static double GetInputDouble(string promt)
    {
        double num;
        Console.Write(promt);
        while (!double.TryParse(Console.ReadLine(), out num))
            return -1;

        return num;
    }

    public static string? GetInputString(string promt)
    {
        string UserInput;
        while (true)
        {
            Console.Write(promt);
            UserInput = (Console.ReadLine() ?? "").Trim();
            return string.IsNullOrEmpty(UserInput) ? null : UserInput;
        }
    }

}
