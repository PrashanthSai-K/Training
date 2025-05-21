using System;
using WholeApplication.Exceptions;
using WholeApplication.Models;

namespace WholeApplication.Repositories;

public class EmployeeRepository : Repository<int, Employee>
{

    public override ICollection<Employee> GetAll()
    {
        if (_items.Count == 0)
            throw new CollectionEmptyException("No Employees found");
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
