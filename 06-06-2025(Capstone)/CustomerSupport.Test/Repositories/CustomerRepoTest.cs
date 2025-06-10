using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using CustomerSupport.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerSupport.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        private ChatDbContext _context;
        private CustomerRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ChatDbContext(options);

            _context.Customers.AddRange(
                new Customer { Id = 1, Name = "Customer A", Email = "a@example.com" },
                new Customer { Id = 2, Name = "Customer B", Email = "b@example.com" }
            );
            _context.SaveChanges();

            _repository = new CustomerRepository(_context);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllCustomers()
        {
            var result = await _repository.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_ValidId_ReturnsCustomer()
        {
            var result = await _repository.GetById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Customer A"));
        }

        [Test]
        public void GetById_InvalidId_ThrowsItemNotFoundException()
        {
            Assert.ThrowsAsync<ItemNotFoundException>(async () => await _repository.GetById(999));
        }

        [Test]
        public async Task Create_ShouldAddCustomer()
        {
            var newCustomer = new Customer { Name = "Customer C", Email = "c@example.com" };

            var result = await _repository.Create(newCustomer);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Customer C"));
            Assert.That(_context.Customers.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Update_ShouldModifyCustomer()
        {
            var updatedCustomer = new Customer { Id = 1, Name = "Customer A Updated", Email = "a.updated@example.com" };

            var result = await _repository.Update(1, updatedCustomer);

            Assert.That(result, Is.Not.Null);
            var dbCustomer = await _repository.GetById(1);
            Assert.That(dbCustomer.Name, Is.EqualTo("Customer A Updated"));
        }

        [Test]
        public async Task Delete_ShouldRemoveCustomer()
        {
            var result = await _repository.Delete(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(_context.Customers.Count(), Is.EqualTo(1));
        }

        [TearDown]
        public void Cleanup()
        {
            _context?.Dispose();
        }
    }
}
