using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(CustomerRegisterDto customerDto)
        {
            var customer = await _customerService.CreateCustomer(customerDto);
            return Created("", customer);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerUpdateDto customerDto)
        {
            var userId = User?.Identity?.Name;

            var customer = await _customerService.UpdateCustomer(userId, id, customerDto);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var userId = User?.Identity?.Name;

            var customer = await _customerService.DeleteCustomer(userId, id);
            return Ok(customer);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            return Ok(customer);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCustomers([FromQuery] CustomerQueryParams queryParams)
        {
            var customers = await _customerService.GetCustomers(queryParams);
            return Ok(customers);
        }
    }
}
