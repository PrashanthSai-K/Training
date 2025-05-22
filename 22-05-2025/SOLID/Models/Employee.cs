using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SOLID.Models;


public class Employee : IComparable<Employee>, IEquatable<Employee>
{
    public int Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public double Salary { get; set; }

    public Employee()
    {
        Name = string.Empty;
    }

    public Employee(int id, int age, string name, double salary)
    {
        Id = id;
        Age = age;
        Name = name;
        Salary = salary;
    }

    //Take Input Details from User Violates SRP SOLID Principle

    public override string ToString()
    {
        return "Employee ID : " + Id + "\nName : " + Name + "\nAge : " + Age + "\nSalary : " + Salary;
    }

    public int CompareTo(Employee? other)
    {
        return this.Id.CompareTo(other?.Id);
    }

    public bool Equals(Employee? other)
    {
        return this.Id == other?.Id;
    }

}

