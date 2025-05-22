using System;
using SOLID.Models;
using SOLID.Interfaces;

namespace SOLID;
//Liskov Principle
public class PermenantEmployee : Employee, ILeaveCount
{
    public int GetAnnualLeave()
    {
        return 30;
    }
}

public class ProbationEmployee : Employee
{

}
