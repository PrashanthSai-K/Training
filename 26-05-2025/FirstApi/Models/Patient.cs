using System;
using System.Runtime.CompilerServices;

namespace FirstApi.Models;

public class Patient
{
    public Patient(int id, string name, int age,string diagnosis)
    {
        Id = id;
        Name = name;
        Age = age;
        Diagnosis = diagnosis;
    }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get;  set; }
    public string Diagnosis { get; set; } = string.Empty;  
}
