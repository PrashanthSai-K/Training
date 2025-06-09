using System;
using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;

namespace CustomerSupport.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<int, Customer> _customerRepository;
    private readonly IRepository<string, User> _userRepository;
    private readonly IHashingService _hashingService;
    private readonly IMapper _mapper;

    public CustomerService(IRepository<int, Customer> customerRepository, IRepository<string, User> userRepository, IMapper mapper, IHashingService hashingService)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _hashingService = hashingService;
        _mapper = mapper;

    }
    public async Task<Customer> CreateCustomer(CustomerRegisterDto customerDto)
    {
        var user = _mapper.Map<CustomerRegisterDto, User>(customerDto);
        user.Password = _hashingService.HashData(user.Password);
        user.Roles = "Customer";
        
        var customer = _mapper.Map<CustomerRegisterDto, Customer>(customerDto);

        var createdUser = await _userRepository.Create(user);
        var createdCustomer = await _customerRepository.Create(customer);

        return createdCustomer;
    }

    public async Task<Customer> DeleteCustomer(int id)
    {
        var deletedCustomer = await _customerRepository.Delete(id);
        var deletedUser = await _userRepository.Delete(deletedCustomer.Email);
        
        return deletedCustomer;
    }

    public async Task<Customer> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetById(id);
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetCustomers()
    {
        var customers = await _customerRepository.GetAll();
        return customers;
    }

    public async Task<Customer> UpdateCustomer(int id, CustomerUpdateDto customerDto)
    {
        var existingCustomer = await _customerRepository.GetById(id);

        var customer = _mapper.Map<CustomerUpdateDto, Customer>(customerDto);
        customer.Id = existingCustomer.Id;
        customer.Email = existingCustomer.Email;

        var updatedCustomer = await _customerRepository.Update(customer.Id, customer);
        return updatedCustomer;
    }
}
