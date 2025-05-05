using Application.Service;
using Core.Entity;
using Core.Repository.Interface;
using Moq;

namespace UnitTest.Application.Service;
public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _mockContactRepository;
    private readonly ContactService _contactService;

    public ContactServiceTests()
    {
        _mockContactRepository = new Mock<IContactRepository>();
        _contactService = new ContactService(_mockContactRepository.Object);
    }

    [Fact]
    public async Task GetAllByDddAsync_ValidDddId_ReturnsContacts()
    {
        // Arrange
        int dddId = 11;        
        var contacts = new List<Contact>
        {
            new() { Id = 1, Name = "Test User 1", Phone = "99983-1617", Email="testUser1@gmail.com", DddId= dddId },
            new() { Id = 2, Name = "Test User 2" , Phone= "99983-1618", Email="testUser2@gmail.com", DddId= dddId }
        };
        
        _mockContactRepository.Setup(repo => repo.GetAllByDddAsync(dddId)).ReturnsAsync(contacts);

        // Act
        var result = await _contactService.GetAllByDddAsync(dddId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllByDddAsync_InvalidDddId_ReturnsEmptyList()
    {
        // Arrange
        int dddId = 999;
        var contacts = new List<Contact>();

        _mockContactRepository.Setup(repo => repo.GetAllByDddAsync(dddId)).ReturnsAsync(contacts);

        // Act
        var result = await _contactService.GetAllByDddAsync(dddId);

        // Act & Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
