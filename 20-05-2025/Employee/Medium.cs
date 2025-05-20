using System;

namespace Employee;

public class Medium
{

    Dictionary<int, Employee> employees = new Dictionary<int, Employee>();

    public static int GetInputNumber(string promt)
    {
        int num;
        Console.Write(promt);
        while (!int.TryParse(Console.ReadLine(), out num))
            Console.WriteLine("Please enter a valid number.");

        return num;
    }

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

    public void AddEmployee()
    {
        int i=1;
        while(true)
        {
            Console.WriteLine($"Please enter emplyee {i} details (Enter STOP to stop or anything else to continue)");
            if(Console.ReadLine() == "STOP")
                break;
            Employee employee = new Employee();
            employee.TakeEmployeeDetailsFromUser();
            if(employees.ContainsKey(employee.Id))
                Console.WriteLine("Id already exixts");
            else
            {
                employees.Add(employee.Id, employee);
                i++;
            }
        }
    }

    public void EditEmployee(int id)
    {
        Employee? emp = FindEmployeeById(id);
        if(emp == null)
        {
            Console.WriteLine($"Employee with Id : {id} not found");
            return; 
        }    
        Employee employee = new Employee();
        employee.TakeEmployeeDetailsFromUser(true);
        employees[id] = employee;
        Console.WriteLine($"Employee with Id : {id} edited successfully");
    }

    public void DeleteEmployee(int id)
    {
        Employee? emp = FindEmployeeById(id);
        if(emp == null)
        {
            Console.WriteLine($"Employee with Id : {id} not found");
            return; 
        }    
        employees.Remove(id);
        Console.WriteLine($"Employee with Id : {id} deleted successfully");
    }

    public Employee? FindEmployeeById(int id)
    {
        if(employees.ContainsKey(id))
            return employees.First(e => e.Key == id).Value;
        return null;
    }

    public Employee? FindEmployeeByName(string name)
    {
        return employees.FirstOrDefault(e=>e.Value.Name == name).Value;
    }

    public List<Employee> FindElderEmployee(string name)
    {
        Employee? employee = FindEmployeeByName(name);
        if (employee == null)
            return new List<Employee>();
        return employees
                    .Where(e=>e.Value.Age > employee.Age)
                    .Select(e=>e.Value)
                    .ToList();
    }

    public void ViewAllEmployees()
    {
        if(!(employees.Count > 0))
        {
            Console.WriteLine("No Employees found");
            return;
        }
        Console.WriteLine("\n--- All Employees ---");
        foreach (var employee in employees.Values)
        {
            Console.WriteLine(employee);
        }
    }

    public override string ToString()
    {
        return string.Join(", ", employees);
    }

    public void Start()
    {
        //1. Create an application that will take employee details (Use the employee class) and store it in a collection  
        AddEmployee();

        // 2. a. Sort the employees based on their salary using IComparale<> interface.  
        List<Employee> employeesList = employees.Values.ToList(); 
        employeesList.Sort();
        Console.WriteLine("\nEmployees sorted by salary:");
        foreach (var emp in employeesList)
        {
            Console.WriteLine(emp);
        }

        //2. b. Given an employee id find the employee and print the details. 

        int id = GetInputNumber("Enter employee ID to find : ");
        Employee? FoundById = FindEmployeeById(id);
        if(FoundById == null)
            Console.WriteLine($"Employee with Id : {id} not found");
        else
            Console.WriteLine(FoundById);

        // 3. Find all the employees with the given name
        string name = GetInputString("Enter employee name to find : ");
        Employee? FoundByName = FindEmployeeByName(name);
        if(FoundByName != null)
            Console.WriteLine(FoundByName);
        else
            Console.WriteLine($"Employee with Name : {name} not found");

        //4. Find all the employees who are elder than a given employee (Employee given by user)
        string name2 = GetInputString("Enter employee name to find elder employees : ");
        List<Employee> ElderEmployees = FindElderEmployee(name2);
        if(ElderEmployees.Count < 1)
            Console.WriteLine("No employee match found forthe given criteria.");
        else
            Console.WriteLine(string.Join("\n",ElderEmployees));
    }
}
