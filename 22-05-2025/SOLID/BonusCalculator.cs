using System;
using SOLID.Models;

namespace SOLID;
/*
    Open / Close Principle
    Open for Extension and Closed for Modification
*/
public class BonusCalculator
{
    //This function should be changed for each and every employee type
    public double CalculateBonus(Employee emp, string type)
    {
        if (type == "Permenant") return emp.Salary * 0.1;
        else if (type == "Probation") return emp.Salary * 0.05;
        return 0;
    }
}

public abstract class EmployeeBonusCalculator
{
    public abstract double CalculateBonus(Employee emp);
}

public class PermenantEmployeeBonus : EmployeeBonusCalculator
{
    public override double CalculateBonus(Employee emp)
    {
        return emp.Salary * 0.1;
    }
}

public class ProbationEmployeeBonus : EmployeeBonusCalculator
{
    public override double CalculateBonus(Employee emp)
    {
        return emp.Salary * 0.05;
    }
}
