using Application.Service;
using Core.Entity;
using Infraestructure.Configuration;
using Infraestructure.Repository;
using IntegrationTest.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.Service
{
    public class DirectDistanceDialingServiceTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly DirectDistanceDialingService _directDistanceDialingService;

        public DirectDistanceDialingServiceTests()
        {
            // Initialize database
            _context = InMemoryDbContextFactory.Create();

            // Initialize repository
            var dddRepository = new DirectDistanceDialingRepository(_context);

            // Initialize service
            _directDistanceDialingService = new DirectDistanceDialingService(dddRepository);

            // Seed database with test data
            SeedDatabase();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetAllAsync_ReturnsAllDirectDistanceDialing()
        {
            // Act
            var result = await _directDistanceDialingService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, d => d.Id == 11);
            Assert.Contains(result, d => d.Id == 12);
            Assert.Contains(result, d => d.Id == 13);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetByIdAsync_ExistingId_ReturnsDirectDistanceDialing()
        {
            //Arrange
            var dddId = 11;

            // Act
            var result = await _directDistanceDialingService.GetByIdAsync(dddId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dddId, result.Id);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int dddId = 999;

            // Act
            var result = await _directDistanceDialingService.GetByIdAsync(dddId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CreateAsync_ValidEntity_CallsRepositoryCreate()
        {
            // Arrange
            var newDdd = new DirectDistanceDialing { Id = 21, Region = "Rio de Janeiro", CreatedOn = DateTime.Now };
                        
            // Act
            await _directDistanceDialingService.CreateAsync(newDdd);
            var result = await _directDistanceDialingService.GetByIdAsync(newDdd.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newDdd.Id, result.Id);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task EditAsync_ValidEntity_CallsRepositoryEdit()
        {
            // Arrange
            var existingDdd = await _directDistanceDialingService.GetByIdAsync(13);
            Assert.NotNull(existingDdd); 

            existingDdd.Region = "São Paulo Updated";

            // Act
            await _directDistanceDialingService.EditAsync(existingDdd);
            var result = await _directDistanceDialingService.GetByIdAsync(existingDdd.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("São Paulo Updated", result.Region);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DeleteAsync_ValidEntity_CallsRepositoryDelete()
        {
            // Arrange
            var dddToDelete = await _directDistanceDialingService.GetByIdAsync(13);
            Assert.NotNull(dddToDelete);

            // Act
            await _directDistanceDialingService.DeleteAsync(dddToDelete);
            var result = await _directDistanceDialingService.GetByIdAsync(dddToDelete.Id);

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            var ddd1 = new DirectDistanceDialing { Id = 11, Region = "São Paulo", CreatedOn = DateTime.Now };
            var ddd2 = new DirectDistanceDialing { Id = 12, Region = "São Paulo", CreatedOn = DateTime.Now };
            var ddd3 = new DirectDistanceDialing { Id = 13, Region = "São Paulo", CreatedOn = DateTime.Now };
            
            _context.DirectDistanceDialings.AddRange(ddd1, ddd2, ddd3);
            _context.SaveChanges();
        }
    }
}
