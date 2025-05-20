using System;

namespace Employee;

public class Hard
{
    private Medium medium;
    public Hard(Medium medium)
    {
        this.medium = medium; 
    }
    public static int GetInputNumber(string promt)
    {
        int num;
        Console.Write(promt);
        while (!int.TryParse(Console.ReadLine(), out num))
            Console.WriteLine("Please enter a valid number.");

        return num;
    }

    //Display a menu to user which will enable to print all the employee details, 
    // add an employee, modify the details of an employee (all except id), 
    // print an employee details given his id and delete an employee from the collection 
    public void Start()
    {
        bool exit = false;
        while(true)
        {
            if(exit)
                break;
            Console.WriteLine("\nPlease select an option : \n 1. Add Employee \n 2. View All Employee \n 3. View By Id \n 4. Modify By Id \n 5. Delete By Id \n Any other no. to stop\n");
            int option = GetInputNumber("");
            switch(option)
            {
                case 1:
                    medium.AddEmployee();
                    break;
                case 2:
                    medium.ViewAllEmployees();
                    break;
                case 3:
                    int FindId = GetInputNumber("Enter Id to find by Id : ");
                    Employee? emp = medium.FindEmployeeById(FindId);
                    if(emp == null)
                        Console.WriteLine("Employee not found.");
                    else
                        Console.WriteLine(emp);
                    break;
                case 4:
                    int EditId = GetInputNumber("Enter Id to edit by Id : ");
                    medium.EditEmployee(EditId);
                    break;
                case 5:
                    int DeleteId = GetInputNumber("Enter Id to delete by Id : ");
                    medium.DeleteEmployee(DeleteId);
                    break;
                default:
                    exit = true;
                    break;
            }
        }
    }
}
