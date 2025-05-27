using System;
using ClinicManagement.Models;

namespace ClinicManagement.Repository;

public class AppointmentRepository : Repository<int, Appointment>
{
    public override int GenerateId()
    {
        var max = _items.Any() ? _items.Max(i => i.Id) : 100;
        return max + 1;
    }

    public override ICollection<Appointment> GetAll()
    {
        if (_items == null || _items.Count == 0)
        {
            throw new Exception("Appointments list is empty");
        }
        return _items.ToList();
    }

    public override Appointment GetById(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id) ?? throw new KeyNotFoundException("Appointment not found for the given Id");
        return item;
    }

}
