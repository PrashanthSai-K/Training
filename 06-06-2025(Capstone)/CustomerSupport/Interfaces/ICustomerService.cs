using System;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;

namespace CustomerSupport.Interfaces;

public interface ICustomerService
{
    public Task<Customer> CreateCustomer(CustomerRegisterDto customer);
    public Task<Customer> UpdateCustomer(string? userId, int id, CustomerUpdateDto customerDto);
    public Task<Customer> DeleteCustomer(string? userId, int id);
    public Task<Customer> GetCustomerById(int id);
    public Task<IEnumerable<Customer>> GetCustomers(CustomerQueryParams queryParams);
}
