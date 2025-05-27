using System;
using ClinicManagement.Models;

namespace ClinicManagement.Repository;

public class PatientRepository : Repository<int, Patient>
{
    public override int GenerateId()
    {
        var max = _items.Any() ? _items.Max(i => i.Id) : 100;
        return max + 1;
    }

    public override ICollection<Patient> GetAll()
    {
        if (_items == null || _items.Count == 0)
        {
            throw new Exception("Patients list is empty");
        }
        return _items.ToList();
    }

    public override Patient GetById(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id) ?? throw new KeyNotFoundException("Patient not found for the given Id");
        return item;
    }
}
