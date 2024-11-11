using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Moq;
using RiK_Test.Server.Data;
using RiK_Test.Server.Repositories;
using RiK_Test.Server.Services;
using RIK_Test.Shared.DTOs;
using RIK_Test.Shared.Models;
using Xunit;

namespace RIK_Test.Tests;
public class EventServiceTests {
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly EventService _eventService;
    private readonly Mock<IParticipantRepository> _participantRepositoryMock;
    private readonly AppDbContext _context;

    public EventServiceTests() {

        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase")
          .Options;
        _context = new AppDbContext(options);
        // Arrange: Set up the mock repository and service
        _eventRepositoryMock = new Mock<IEventRepository>();
        _participantRepositoryMock = new Mock<IParticipantRepository>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _eventService = new EventService(_context, _eventRepositoryMock.Object, _participantRepositoryMock.Object, mapper);
    }

    public void Dispose() => _context.Database.EnsureDeleted();


    [Fact]
    public async Task GetEventsAsync() {
        // Arrange
        var events = new List<Event>
        {
        new Event { Id = 1, Name = "Test event 1", Date = DateTime.Now.AddDays(-5) },
        new Event { Id = 2, Name = "Test event 2", Date = DateTime.Now.AddDays(10) }
    };

        _eventRepositoryMock.Setup(repo => repo.GetEventsAsync()).ReturnsAsync(events);

        // Act
        var result = await _eventService.GetEventsAsync(null);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, e => e.Name == "Test event 1");
        Assert.Contains(result, e => e.Name == "Test event 2");
    }

    [Fact]
    public async Task GetEventByIdAsync() {
        // Arrange
        var evt = new Event { Id = 1, Name = "Test Event" };
        _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(evt);

        // Act
        var result = await _eventService.GetEventByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Event", result.Name);
    }

    [Fact]
    public async Task AddEventAsync() {
        // Arrange
        var evt = new Event { Id = 1, Name = "Test Event" };
        _eventRepositoryMock.Setup(repo => repo.AddEventAsync(evt)).Returns(Task.CompletedTask);
        _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(evt);

        // Act
        var result = await _eventService.AddEventAsync(evt);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Event", result.Name);
    }

    [Fact]
    public async Task DeleteEventAsync() {
        // Arrange
        _eventRepositoryMock.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);
        _eventRepositoryMock.Setup(repo => repo.GetEventsAsync()).ReturnsAsync(new List<Event>());

        // Act
        var result = await _eventService.DeleteEventAsync(1);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task RegisterOrAddParticipantToEvent() {
        // Arrange
        var evt = new Event { Id = 1, Name = "Test Event" };
        var participantDto = new CreateParticipantDTO { Name = "John Doe", Code = "1234" };
        _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(evt);
        _participantRepositoryMock.Setup(repo => repo.GetByCodeAsync("1234")).ReturnsAsync((Participant?)null);

        // Act
        var result = await _eventService.RegisterOrAddParticipantToEvent(1, participantDto);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result.Participants, p => p.Name == "John Doe");
    }
}
