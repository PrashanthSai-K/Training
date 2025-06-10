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
    public class ChatMessagesRepositoryTests
    {
        private ChatDbContext _context;
        private ChatMessagesRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ChatDbContext(options);

            _context.ChatMessages.AddRange(
                new ChatMessage
                {
                    Id = 1,
                    ChatId = 1,
                    UserId = "user1@gmail.com",
                    MessageType = MessageType.Message,
                    Message = "Hello!",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-5)
                },
                new ChatMessage
                {
                    Id = 2,
                    ChatId = 1,
                    UserId = "user2@gmail.com",
                    MessageType = MessageType.Image,
                    ImageName = "image.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            );
            _context.SaveChanges();

            _repository = new ChatMessagesRepository(_context);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllChatMessages()
        {
            var result = await _repository.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_ValidId_ReturnsChatMessage()
        {
            var result = await _repository.GetById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Hello!"));
        }

        [Test]
        public void GetById_InvalidId_ThrowsItemNotFoundException()
        {
            Assert.ThrowsAsync<ItemNotFoundException>(async () => await _repository.GetById(999));
        }

        [Test]
        public async Task Create_ShouldAddChatMessage()
        {
            var newMessage = new ChatMessage
            {
                ChatId = 2,
                UserId = "user3@gmail.com",
                MessageType = MessageType.Message,
                Message = "How are you?"
            };

            var result = await _repository.Create(newMessage);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("How are you?"));
            Assert.That(_context.ChatMessages.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Update_ShouldModifyChatMessage()
        {
            var updatedMessage = new ChatMessage
            {
                Id = 1,
                ChatId = 1,
                UserId = "user1@gmail.com",
                MessageType = MessageType.Message,
                Message = "Updated Hello!",
                CreatedAt = DateTime.UtcNow
            };

            var result = await _repository.Update(1, updatedMessage);

            Assert.That(result, Is.Not.Null);
            var dbMessage = await _repository.GetById(1);
            Assert.That(dbMessage.Message, Is.EqualTo("Updated Hello!"));
        }

        [Test]
        public async Task Delete_ShouldRemoveChatMessage()
        {
            var result = await _repository.Delete(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(_context.ChatMessages.Count(), Is.EqualTo(1));
        }

        [TearDown]
        public void Cleanup()
        {
            _context?.Dispose();
        }
    }
}
