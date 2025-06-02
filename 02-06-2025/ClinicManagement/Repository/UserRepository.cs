using System;
using ClinicManagement.Context;
using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Repository;

public class UserRepository : Repository<string, User>
{
    public UserRepository(ClinicDBContext clinicDBContext) : base(clinicDBContext)
    {
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        var users = await _clinicDBContext.Users.ToListAsync();
        return (users == null || users.Count == 0) ? throw new Exception("No Speciality found in db") : users;
    }

    public override async Task<User> GetById(string name)
    {
        var speciality = await _clinicDBContext.Users.SingleOrDefaultAsync(u => u.UserName == name);
        return speciality ?? throw new Exception($"User With username : {name} not found");
    }
}
