using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using RiK_Test.Server.Repositories;
using RiK_Test.Server.Services;
using RIK_Test.Shared.DTOs;
using RIK_Test.Shared.Models;
using Xunit;

namespace RiK_Test.Tests.Services;
public class ParticipantServiceTests {
    private readonly Mock<IParticipantRepository> _participantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ParticipantService _participantService;

    public ParticipantServiceTests() {
        _participantRepositoryMock = new Mock<IParticipantRepository>();
        _mapperMock = new Mock<IMapper>();
        _participantService = new ParticipantService(_participantRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task DeleteParticipantAsync_WhenCalled_DeletesParticipantAndReturnsUpdatedList() {
        // Arrange
        var participantId = 1;
        var participants = new List<Participant>
        {
        new Participant { Id = 1, Name = "Test Participant" },
        new Participant { Id = 2, Name = "Test2 Participant" }
    };

        _participantRepositoryMock.Setup(repo => repo.DeleteAsync(participantId))
                                  .Returns(Task.CompletedTask);

        _participantRepositoryMock.Setup(repo => repo.GetAllAsync())
                                  .ReturnsAsync(participants);

        // Act
        var result = await _participantService.DeleteParticipantAsync(participantId);

        // Assert
        _participantRepositoryMock.Verify(repo => repo.DeleteAsync(participantId), Times.Once);
        _participantRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        Assert.Equal(participants, result);
    }

    [Fact]
    public async Task UpdateParticipantAsync_WhenParticipantExists_UpdatesParticipantAndReturnsDto() {
        // Arrange
        var participantId = 1;
        var existingParticipant = new Participant {
            Id = participantId,
            Name = "Name",
            LastName = "LastName",
            PaymentMethod = "Payment",
            Description = "Description",
            Type = Participant.ParticipantType.Private,
            Code = "Code",
            ParticipantCount = 10
        };

        var updateDto = new CreateParticipantDTO {
            Name = "New Name",
            LastName = "New LastName",
            PaymentMethod = "New Payment",
            Description = "New Description",
            Type = Participant.ParticipantType.Business,
            Code = "New Code",
            ParticipantCount = 20
        };

        var expectedDto = new CreateParticipantDTO {
            Name = "New Name",
            LastName = "New LastName",
            PaymentMethod = "New Payment",
            Description = "New Description",
            Type = Participant.ParticipantType.Business,
            Code = "New Code",
            ParticipantCount = 20
        };
        _participantRepositoryMock.Setup(repo => repo.GetByIdAsync(participantId))
                                  .ReturnsAsync(existingParticipant);

        _mapperMock.Setup(mapper => mapper.Map<CreateParticipantDTO>(existingParticipant))
                   .Returns(expectedDto);

        // Act
        var result = await _participantService.UpdateParticipantAsync(participantId, updateDto);

        // Assert
        _participantRepositoryMock.Verify(repo => repo.GetByIdAsync(participantId), Times.Once);
        _participantRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Participant>(p =>
            p.Name == updateDto.Name &&
            p.LastName == updateDto.LastName &&
            p.PaymentMethod == updateDto.PaymentMethod &&
            p.Description == updateDto.Description &&
            p.Type == updateDto.Type &&
            p.Code == updateDto.Code &&
            p.ParticipantCount == updateDto.ParticipantCount
        )), Times.Once);

        Assert.Equal(expectedDto, result);
    }

}
