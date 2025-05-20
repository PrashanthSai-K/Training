using System;

namespace Employee;

public class EmployeePromotion
{
    List<string> employees = new List<string>();

    public static string GetInputString(string promt)
    {
        string UserInput;
        while (true)
        {
            Console.Write(promt);
            UserInput = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrEmpty(UserInput))
            {
                Console.WriteLine("Please Enter a valid input.");
            }
            else
            {
                return UserInput;
            }
        }
    }

    public void AddEmployees()
    {
        string name = GetInputString("Enter employee name : ");
        while(true)
        {
            if(name == "STOP")
                break;
            employees.Add(name);
            name = GetInputString("Enter employee name : ");
        }
    }

    public int FindPosition(string name)
    {
        return employees.IndexOf(name)+1;
    }

    public override string ToString()
    {
        return string.Join(", ", employees);
    }

    public void Start()
    {
        Console.WriteLine("Please enter the employee names in the order of their eligibility for promotion(Please enter 'STOP' to stop) ");
        // 1. Create a collection that will hold the employee names in the same order that they are inserted. 
        AddEmployees();
        Console.WriteLine("\nEmployees listed for promotion : ");
        Console.WriteLine($"    {ToString()}");

        // 2. Given an employee name find his position in the promotion list 
        string name = GetInputString("\nPlease enter the name of the employee to check promotion position : ");
        int Position = FindPosition(name);
        if(Position == 0)
            Console.WriteLine($"    {name} not found on the list");
        else
            Console.WriteLine($"    {name} is in the position {Position} for promotion");

        // 3. The application seems to be using some excess memory for storing the name, 
        // contain the space by using only the quantity of memory that is required.
        employees.RemoveRange(1, 1);
        Console.WriteLine($"Before : Count {employees.Count}, Capacity {employees.Capacity}");
        employees.TrimExcess();
        Console.WriteLine($"After : Count {employees.Count}, Capacity {employees.Capacity}");


        // 4. Print all the employee names in ascending order. 
        employees.Sort();
        Console.WriteLine("\nPromoted List in Order : ");
        Console.WriteLine($"    {ToString()}");

    }
}
