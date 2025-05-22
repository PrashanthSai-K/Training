using System;
using SOLID.Models;
using SOLID.Repositories;

namespace SOLID.Repositories;

public class EmployeeRepository : Repository<int, Employee>
{

    public override ICollection<Employee> GetAll()
    {
        if (_items.Count == 0)
            throw new KeyNotFoundException("No Employees found");
        return _items;
    }

    public override Employee GetById(int id)
    {
        var emp = _items.FirstOrDefault(e => e.Id == id) ?? throw new KeyNotFoundException();
        return emp;
    }

    protected override int GenerateID()
    {
        if (_items.Count == 0)
            return 101;
        return _items.Max(e => e.Id) + 1;
    }
}
