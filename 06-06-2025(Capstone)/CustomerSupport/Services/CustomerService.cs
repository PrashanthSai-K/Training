using System;
using AutoMapper;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;

namespace CustomerSupport.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<int, Customer> _customerRepository;
    private readonly IRepository<string, User> _userRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly IHashingService _hashingService;
    private readonly IMapper _mapper;

    public CustomerService(IRepository<int, Customer> customerRepository,
                            IRepository<string, User> userRepository,
                            IMapper mapper,
                            IAuditLogService auditLogService,
                            IHashingService hashingService)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _auditLogService = auditLogService;
        _hashingService = hashingService;
        _mapper = mapper;

    }
    public async Task<Customer> CreateCustomer(CustomerRegisterDto customerDto)
    {
        var user = _mapper.Map<CustomerRegisterDto, User>(customerDto);
        user.Password = _hashingService.HashData(customerDto.Password);
        user.Roles = "Customer";

        var customer = _mapper.Map<CustomerRegisterDto, Customer>(customerDto);

        var createdUser = await _userRepository.Create(user);
        var createdCustomer = await _customerRepository.Create(customer);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = user.Username, Action = "Create", Entity = "Customer", CreatedOn = DateTime.UtcNow });

        return createdCustomer;
    }

    public async Task<Customer> DeleteCustomer(string? userId, int id)
    {
        var existingCustomer = await _customerRepository.GetById(id);

        if (existingCustomer.Email != userId)
            throw new UnauthorizedAccessException("User not authorized to update this account details");

        var deletedCustomer = await _customerRepository.Delete(id);
        var deletedUser = await _userRepository.Delete(deletedCustomer.Email);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Delete", Entity = "Customer", CreatedOn = DateTime.UtcNow });

        return deletedCustomer;
    }

    public async Task<Customer> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetById(id);
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetCustomers(CustomerQueryParams queryParams)
    {
        var customers = await _customerRepository.GetAll();
        customers = GetCustomerByName(customers, queryParams.Name);
        customers = GetCustomerByEmail(customers, queryParams.Email);

        customers = customers.Skip((queryParams.PageNumber - 1) * queryParams.PageSize).Take(queryParams.PageSize);
        return customers;
    }

    public async Task<Customer> UpdateCustomer(string? userId, int id, CustomerUpdateDto customerDto)
    {
        var existingCustomer = await _customerRepository.GetById(id);

        if (existingCustomer.Email != userId)
            throw new UnauthorizedAccessException("User not authorized to update this account details");

        var customer = _mapper.Map<CustomerUpdateDto, Customer>(customerDto);
        customer.Id = existingCustomer.Id;
        customer.Email = existingCustomer.Email;

        var updatedCustomer = await _customerRepository.Update(customer.Id, customer);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Update", Entity = "Customer", CreatedOn = DateTime.UtcNow });

        return updatedCustomer;
    }

    public IEnumerable<Customer> GetCustomerByName(IEnumerable<Customer> customers, string? name)
    {
        if (name == null || name.Length == 0)
            return customers;

        return customers.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public IEnumerable<Customer> GetCustomerByEmail(IEnumerable<Customer> customers, string? email)
    {
        if (email == null || email.Length == 0)
            return customers;

        return customers.Where(a => a.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
    }

}
