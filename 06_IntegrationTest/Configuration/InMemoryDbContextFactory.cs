using Infraestructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest.Configuration
{
    public static class InMemoryDbContextFactory
    {
        public static DataContext Create()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("DataSource=:memory:") // Use SQLite in-memory
                .Options;

            var context = new DataContext(options);
            context.Database.OpenConnection(); // Keep connection open for in-memory DB
            context.Database.EnsureCreated(); // Apply migrations if necessary

            // Clear seeding database for integration tests
            context.DirectDistanceDialings.RemoveRange(context.DirectDistanceDialings);
            context.SaveChanges();

            return context;
        }
    }
}
