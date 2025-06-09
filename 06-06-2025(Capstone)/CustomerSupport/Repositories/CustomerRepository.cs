using System;
using ClinicManagement.Repository;
using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Repositories;

public class CustomerRepository : Repository<int, Customer>
{
    public CustomerRepository(ChatDbContext chatDbContext) : base(chatDbContext)
    {
    }

    public override async Task<IEnumerable<Customer>> GetAll()
    {
        var customers = await _chatDbContext.Customers.ToListAsync()
                ?? throw new EntityEmptyException("No Customers found in database");
        return customers;

    }

    public override async Task<Customer> GetById(int id)
    {
        var customer = await _chatDbContext.Customers.FirstOrDefaultAsync(a => a.Id == id)
                        ?? throw new ItemNotFoundException($"Customer with Id : {id} not found");
        return customer;
    }
}
