using System;
using ClinicManagement.Models;

namespace ClinicManagement.Repository;

public class DoctorRepository : Repository<int, Doctor>
{
    public override int GenerateId()
    {
        var max = _items.Any() ? _items.Max(i => i.Id) : 100;
        return max + 1;
    }

    public override ICollection<Doctor> GetAll()
    {
        if (_items == null || _items.Count == 0)
        {
            throw new Exception("Doctors list is empty");
        }
        return _items.ToList();
    }

    public override Doctor GetById(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id) ?? throw new KeyNotFoundException("Doctor not found for the given Id");
        return item;
    }
}
