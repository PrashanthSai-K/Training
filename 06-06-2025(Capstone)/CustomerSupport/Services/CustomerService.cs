using System;
using AutoMapper;
using CustomerSupport.Exceptions;
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
    private readonly IOtherContextFunctions _otherContextFunctionalities;
    private readonly IMapper _mapper;

    public CustomerService(IRepository<int, Customer> customerRepository,
                            IRepository<string, User> userRepository,
                            IMapper mapper,
                            IAuditLogService auditLogService,
                            IOtherContextFunctions otherContextFunctions,
                            IHashingService hashingService)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _auditLogService = auditLogService;
        _hashingService = hashingService;
        _otherContextFunctionalities = otherContextFunctions;
        _mapper = mapper;

    }
    public async Task<Customer> CreateCustomer(CustomerRegisterDto customerDto)
    {
        if (await _otherContextFunctionalities.IsUsernameExists(customerDto.Email))
            throw new DuplicateEntryException("Email already exists");

        var user = _mapper.Map<CustomerRegisterDto, User>(customerDto);
        user.Password = _hashingService.HashData(customerDto.Password);
        user.Roles = "Customer";
        user.Status = "Active";

        var customer = _mapper.Map<CustomerRegisterDto, Customer>(customerDto);
        customer.Status = "Active";

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

        existingCustomer.Status = "Deleted";
        var deletedCustomer = await _customerRepository.Update(id, existingCustomer);

        var deletedUser = await _userRepository.GetById(deletedCustomer.Email);
        deletedUser.Status = "Deleted";
        await _userRepository.Update(existingCustomer.Email, deletedUser);

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
        customers = GetCustomerByQuery(customers, queryParams.Query);

        customers = customers.Skip((queryParams.PageNumber - 1) * queryParams.PageSize).Take(queryParams.PageSize);
        return customers;
    }

    public async Task<Customer> UpdateCustomer(string? userId, int id, CustomerUpdateDto customerDto)
    {
        var existingCustomer = await _customerRepository.GetById(id);

        var user = await _userRepository.GetById(userId ?? "");

        if (existingCustomer.Email != userId && user.Roles != "Admin")
            throw new UnauthorizedAccessException("User not authorized to update this account details");

        var customer = _mapper.Map<CustomerUpdateDto, Customer>(customerDto);
        customer.Id = existingCustomer.Id;
        customer.Email = existingCustomer.Email;
        customer.Status = existingCustomer.Status;

        var updatedCustomer = await _customerRepository.Update(customer.Id, customer);
        await _auditLogService.CreateAuditLog(new AuditLog() { UserId = userId, Action = "Update", Entity = "Customer", CreatedOn = DateTime.UtcNow });

        return updatedCustomer;
    }

    public IEnumerable<Customer> GetCustomerByQuery(IEnumerable<Customer> customers, string? query)
    {
        if (query == null || query.Length == 0 || customers.Count() == 0)
            return customers;

        return customers.Where(a => a.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || a.Email.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public IEnumerable<Customer> GetCustomerByEmail(IEnumerable<Customer> customers, string? email)
    {
        if (email == null || email.Length == 0)
            return customers;

        return customers.Where(a => a.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<Customer> ActivateCustomer(string? userId, int id)
    {
        var user = await _userRepository.GetById(userId ?? "");
        var customer = await _customerRepository.GetById(id);

        if (customer.Email != userId && user.Roles != "Admin")
            throw new UnauthorizedAccessException("User not authorized to activate the account");

        customer.Status = "Active";
        var updatedCustomer = await _customerRepository.Update(id, customer);
        var agentUser = await _userRepository.GetById(customer.Email);
        agentUser.Status = "Active";
        await _userRepository.Update(customer.Email, agentUser);

        return updatedCustomer;
    }

    public async Task<Customer> DeactivateCustomer(string? userId, int id)
    {
        var user = await _userRepository.GetById(userId ?? "");
        var customer = await _customerRepository.GetById(id);

        if (customer.Email != userId && user.Roles != "Admin")
            throw new UnauthorizedAccessException("User not authorized to activate the account");

        customer.Status = "Inactive";
        var updatedCustomer = await _customerRepository.Update(id, customer);
        var agentUser = await _userRepository.GetById(customer.Email);
        agentUser.Status = "Inactive";
        await _userRepository.Update(customer.Email, agentUser);

        return updatedCustomer;
    }

}
