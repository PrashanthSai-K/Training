using System;
using SOLID.Models;

namespace Delegates;

public class EmployeeInputHandler
{

    public Employee TakeEmployeeDetailsFromUser()
    {
        Employee emp = new Employee();

        Console.WriteLine("Please enter the employee name");
        emp.Name = Console.ReadLine() ?? "";
        Console.WriteLine("Please enter the employee age");
        int age;
        while (!int.TryParse(Console.ReadLine(), out age) || age < 18)
        {
            Console.WriteLine("Invalid entry for age. Please enter a valid employee age");
        }
        emp.Age = age;
        Console.WriteLine("Please enter the employee salary");
        float salary;
        while (!float.TryParse(Console.ReadLine(), out salary) || salary <= 0)
        {
            Console.WriteLine("Invalid entry for salary. Please enter a valid employee salary");
        }
        emp.Salary = salary;
        return emp;
    }

}
