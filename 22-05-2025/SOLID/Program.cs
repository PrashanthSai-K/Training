using Delegates;
using SOLID.Models;
using SOLID;
using SOLID.Interfaces;
using SOLID.Repositories;
using SOLID.Services;
using SOLID.Services;

public class Solid
{
    public static void Main(string[] args)
    {
        Employee employee = new EmployeeInputHandler().TakeEmployeeDetailsFromUser(); // Single Responsibility Principle

        BonusCalculator calcBonus = new BonusCalculator(); // Open / Close Principle
        Console.WriteLine($"\nPermenantEmployee Bonus : {calcBonus.CalculateBonus(employee, "Permenant")}");

        EmployeeBonusCalculator BonusCalc = new PermenantEmployeeBonus();
        Console.WriteLine($"\nPermenantEmployee Bonus : {BonusCalc.CalculateBonus(employee)}");

        PermenantEmployee PermenantEmp = new PermenantEmployee(); //Liskov Principle
        ProbationEmployee ProbationEmp = new ProbationEmployee();

        Console.WriteLine($"\nPermenant Employee Annual Leave Count :  {PermenantEmp.GetAnnualLeave()}");
        // ProbationEmp Doesnot contains GetAnnualLeave method as it does not have it

        DeveloperWork DevWork = new DeveloperWork(); //Interface Segregation Principle
        HRWork HrWork = new HRWork();

        DevWork.Code();
        HrWork.ConductMeeting();


        IRepository<int, Employee> employeeRepo = new EmployeeRepository(); //Dependency Inversion Principle
        IEmployeeService employeeService = new EmployeeService(employeeRepo);
        EmployeeApp employeeApp = new EmployeeApp(employeeService);
        employeeApp.Start();

    }
}