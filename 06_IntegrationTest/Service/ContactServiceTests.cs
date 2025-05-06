using Application.Service;
using Core.Entity;
using Infraestructure.Configuration;
using Infraestructure.Repository;
using IntegrationTest.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.Service
{
    public class ContactServiceTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            // Initialize database
            _context = InMemoryDbContextFactory.Create();

            // Initialize repositories
            var contactRepository = new ContactRepository(_context);

            // Initialize service
            _contactService = new ContactService(contactRepository);

            // Seed database with test data
            SeedDatabase();
        }


        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetAllByDddAsync_ValidDddId_ReturnsContacts()
        {
            // Act
            var result = await _contactService.GetAllByDddAsync(11);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "User 1");
            Assert.Contains(result, c => c.Name == "User 2");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetAllByDddAsync_InvalidDddId_ReturnsEmptyList()
        {
            // Act
            var result = await _contactService.GetAllByDddAsync(999);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            var ddd = new DirectDistanceDialing { Id = 11, Region = "São Paulo", CreatedOn = DateTime.Now };
            _context.DirectDistanceDialings.Add(ddd);

            var contact1 = new Contact { Id = 1, Name = "User 1", Phone = "99983-1617", Email = "user1@gmail.com", DddId = 11 };
            var contact2 = new Contact { Id = 2, Name = "User 2", Phone = "99983-1618", Email = "user2@gmail.com", DddId = 11 };

            _context.Contacts.AddRange(contact1, contact2);
            _context.SaveChanges();
        }
    }
}
