using System;

namespace FirstApi.Models;

public class Doctor
{
    public Doctor(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

}
