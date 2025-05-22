using System;
using SOLID.Interfaces;

namespace SOLID;

public class DeveloperWork : IDeveloperWork
{
    public void work()
    {
        Console.WriteLine("Developer Work is in Progres.....");
    }
    public void Code()
    {
        Console.WriteLine("Coding is in Progres.....");
    }

}

public class HRWork : IHRWork
{
    public void work()
    {
        Console.WriteLine("Hr Work is in progress...");
    }
    public void ConductMeeting()
    {
        Console.WriteLine("Meeting will be conducted in the evening.");
    }
}

public class ManagerWork : IEmployeeWork
{
    public void Code()
    {
        Console.WriteLine("Coding is in Progres.....");
    }

    public void ConductMeeting()
    {
        Console.WriteLine("Meeting will be conducted in the evening.");
    }

    public void Work()
    {
        Console.WriteLine("Developer Work is in Progres.....");
    }
}
