using CustomerSupport.Context;
using CustomerSupport.Exceptions;
using CustomerSupport.Models;
using CustomerSupport.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerSupport.Tests.Repositories
{
    public class AgentRepositoryTests
    {
        private ChatDbContext _context;
        private AgentRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ChatDbContext(options);

            // Seed sample data
            _context.Agents.AddRange(
                new Agent { Id = 1, Name = "Agent A", Email = "a@example.com" },
                new Agent { Id = 2, Name = "Agent B", Email = "b@example.com" }
            );
            _context.SaveChanges();

            _repository = new AgentRepository(_context);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllAgents()
        {
            var result = await _repository.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_ValidId_ReturnsAgent()
        {
            var result = await _repository.GetById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Agent A"));
        }

        [Test]
        public void GetById_InvalidId_ThrowsItemNotFoundException()
        {
            Assert.ThrowsAsync<ItemNotFoundException>(async () => await _repository.GetById(99));
        }

        [Test]
        public async Task Create_ShouldAddAgent()
        {
            var newAgent = new Agent { Name = "Agent C", Email = "c@example.com" };

            var result = await _repository.Create(newAgent);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Agent C"));
            Assert.That(_context.Agents.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Update_ShouldModifyAgent()
        {
            var updatedAgent = new Agent {Id = 1,  Name = "Agent A Updated", Email = "a.updated@example.com" };

            var result = await _repository.Update(1, updatedAgent);

            Assert.That(result, Is.Not.Null);
            var dbAgent = await _repository.GetById(1);
            Assert.That(dbAgent.Name, Is.EqualTo("Agent A Updated"));
        }

        [Test]
        public async Task Delete_ShouldRemoveAgent()
        {
            var result = await _repository.Delete(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(_context.Agents.Count(), Is.EqualTo(1));
        }
        [TearDown]
        public void Cleanup()
        {
            _context?.Dispose();
        }
    }
}
