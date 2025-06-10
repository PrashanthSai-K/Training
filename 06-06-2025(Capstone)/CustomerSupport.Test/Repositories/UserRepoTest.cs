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
    public class UserRepositoryTests
    {
        private ChatDbContext _context;
        private UserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ChatDbContext(options);

            _context.Users.AddRange(
                new User { Username = "user1", Password = "pass1", Roles = "Agent" },
                new User { Username = "user2", Password = "pass2", Roles = "Customer" }
            );
            _context.SaveChanges();

            _repository = new UserRepository(_context);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllUsers()
        {
            var result = await _repository.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_ValidUsername_ReturnsUser()
        {
            var result = await _repository.GetById("user1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Roles, Is.EqualTo("Agent"));
        }

        [Test]
        public void GetById_InvalidUsername_ThrowsItemNotFoundException()
        {
            Assert.ThrowsAsync<ItemNotFoundException>(async () => await _repository.GetById("unknownUser"));
        }

        [Test]
        public async Task Create_ShouldAddUser()
        {
            var newUser = new User
            {
                Username = "user3",
                Password = "pass3",
                Roles = "Admin"
            };

            var result = await _repository.Create(newUser);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo("user3"));
            Assert.That(_context.Users.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Update_ShouldModifyUser()
        {
            var updatedUser = new User
            {
                Username = "user1",
                Password = "updatedpass",
                Roles = "Agent"
            };

            var result = await _repository.Update("user1", updatedUser);

            Assert.That(result, Is.Not.Null);
            var dbUser = await _repository.GetById("user1");
            Assert.That(dbUser.Password, Is.EqualTo("updatedpass"));
        }

        [Test]
        public async Task Delete_ShouldRemoveUser()
        {
            var result = await _repository.Delete("user2");

            Assert.That(result, Is.Not.Null);
            Assert.That(_context.Users.Count(), Is.EqualTo(1));
        }

        [TearDown]
        public void Cleanup()
        {
            _context?.Dispose();
        }
    }
}
