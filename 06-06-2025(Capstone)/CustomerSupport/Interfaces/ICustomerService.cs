using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Interfaces;

public interface ICustomerService
{
    public Task<Customer> CreateCustomer(CustomerRegisterDto customer);
    public Task<Customer> UpdateCustomer(int id, CustomerUpdateDto customerDto);
    public Task<Customer> DeleteCustomer(int id);
    public Task<Customer> GetCustomerById(int id);
    public Task<IEnumerable<Customer>> GetCustomers();
}
