using CustomerSupport.Controllers;
using CustomerSupport.Interfaces;
using CustomerSupport.Models;
using CustomerSupport.Models.Dto;
using CustomerSupport.Models.QueryParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerSupport.Test.Controllers
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _customerServiceMock;
        private CustomerController _controller;

        [SetUp]
        public void Setup()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _controller = new CustomerController(_customerServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "customer123"),
                new Claim(ClaimTypes.Role, "Customer")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task RegisterCustomer_ReturnsCreated()
        {
            var dto = new CustomerRegisterDto();
            _customerServiceMock.Setup(s => s.CreateCustomer(dto)).ReturnsAsync(new Customer());

            var result = await _controller.RegisterCustomer(dto);

            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task UpdateCustomer_ReturnsOk()
        {
            var dto = new CustomerUpdateDto();
            _customerServiceMock.Setup(s => s.UpdateCustomer("customer123", 1, dto)).ReturnsAsync(new Customer());

            var result = await _controller.UpdateCustomer(1, dto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteCustomer_ReturnsOk()
        {
            _customerServiceMock.Setup(s => s.DeleteCustomer("customer123", 1)).ReturnsAsync(new Customer());

            var result = await _controller.DeleteCustomer(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetCustomerById_ReturnsOk()
        {
            _customerServiceMock.Setup(s => s.GetCustomerById(1)).ReturnsAsync(new Customer());

            var result = await _controller.GetCustomerById(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetCustomers_ReturnsOk()
        {
            _customerServiceMock.Setup(s => s.GetCustomers(It.IsAny<CustomerQueryParams>()))
                                .ReturnsAsync(new List<Customer>());

            var result = await _controller.GetCustomers(new CustomerQueryParams());

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
