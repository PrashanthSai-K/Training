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
    public class ChatRepositoryTests
    {
        private ChatDbContext _context;
        private ChatRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ChatDbContext(options);

            _context.Chats.AddRange(
                new Chat
                {
                    Id = 1,
                    AgentId = 1,
                    CustomerId = 1,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    Status = "Open"
                },
                new Chat
                {
                    Id = 2,
                    AgentId = 2,
                    CustomerId = 2,
                    CreatedOn = DateTime.UtcNow,
                    Status = "Closed"
                }
            );
            _context.SaveChanges();

            _repository = new ChatRepository(_context);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllChats()
        {
            var result = await _repository.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_ValidId_ReturnsChat()
        {
            var result = await _repository.GetById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo("Open"));
        }

        [Test]
        public void GetById_InvalidId_ThrowsItemNotFoundException()
        {
            Assert.ThrowsAsync<ItemNotFoundException>(async () => await _repository.GetById(999));
        }

        [Test]
        public async Task Create_ShouldAddChat()
        {
            var newChat = new Chat
            {
                AgentId = 3,
                CustomerId = 3,
                CreatedOn = DateTime.UtcNow,
                Status = "Pending"
            };

            var result = await _repository.Create(newChat);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo("Pending"));
            Assert.That(_context.Chats.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Update_ShouldModifyChat()
        {
            var updatedChat = new Chat
            {
                Id = 1,
                AgentId = 1,
                CustomerId = 1,
                CreatedOn = DateTime.UtcNow,
                Status = "Resolved"
            };

            var result = await _repository.Update(1, updatedChat);

            Assert.That(result, Is.Not.Null);
            var dbChat = await _repository.GetById(1);
            Assert.That(dbChat.Status, Is.EqualTo("Resolved"));
        }

        [Test]
        public async Task Delete_ShouldRemoveChat()
        {
            var result = await _repository.Delete(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(_context.Chats.Count(), Is.EqualTo(1));
        }

        [TearDown]
        public void Cleanup()
        {
            _context?.Dispose();
        }
    }
}
