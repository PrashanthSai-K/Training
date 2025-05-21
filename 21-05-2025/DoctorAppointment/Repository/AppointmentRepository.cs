using System;
using DoctorAppointment.Exceptions;
using DoctorAppointment.Models;

namespace DoctorAppointment.Repository;

public class AppointmentRepository : Repository<int, PatitentAppointment>
{
    public override ICollection<PatitentAppointment> GetAll()
    {
        if (_items.Count < 1)
            throw new CollectionEmptyException("No Appointments found");
        return _items;
    }

    public override PatitentAppointment GetById(int id)
    {
        var patient = _items.FirstOrDefault(i => i.Id == id) ?? throw new KeyNotFoundException($"Appointment with {id} not found");
        return patient;
    }

    protected override int GenerateId()
    {
        if (_items.Count == 0)
            return 101;
        return _items.Max(i => i.Id) + 1;
    }
}
