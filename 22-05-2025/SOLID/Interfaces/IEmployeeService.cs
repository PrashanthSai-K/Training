using System;
using SOLID.Models;

namespace SOLID.Interfaces;

public interface IEmployeeService
{
    int AddEmployee(Employee employee);
    List<Employee> SearchEmployee(SearchModel searchModel);

}
