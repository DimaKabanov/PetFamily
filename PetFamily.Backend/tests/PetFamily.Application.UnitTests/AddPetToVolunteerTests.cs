using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.Commands.AddPetToVolunteer;
using PetFamily.Domain.Shared;
using PetFamily.UnitTests.Infrastructure;

namespace PetFamily.Application.UnitTests;

public class AddPetToVolunteerTests
{
    private readonly IVolunteersRepository _volunteersRepositoryMock = Substitute.For<IVolunteersRepository>();
    private readonly IReadDbContext _readDbContextMock = Substitute.For<IReadDbContext>();
    private readonly IValidator<AddPetToVolunteerCommand> _validatorMock =  Substitute.For<IValidator<AddPetToVolunteerCommand>>();
    private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    private readonly ILogger<AddPetToVolunteerService> _loggerMock = Substitute.For<ILogger<AddPetToVolunteerService>>();
    
    [Fact]
    public async Task Add_Pet_To_Volunteer_Should_Be_Success()
    {
        // arrange
        var ct = new CancellationTokenSource().Token;
        var volunteer = VolunteerFactory.CreateVolunteer();
        var command = VolunteerFactory.CreateAddPetToVolunteerCommand(volunteer.Id.Value);
        
        _volunteersRepositoryMock.GetById(volunteer.Id, ct).Returns(volunteer);
        _validatorMock.ValidateAsync(command, ct).Returns(new ValidationResult());
        _unitOfWorkMock.SaveChanges(ct).Returns(Task.CompletedTask);
        _loggerMock.LogInformation("Success");
        
        var service  = new AddPetToVolunteerService(
            _volunteersRepositoryMock,
            _readDbContextMock,
            _validatorMock,
            _unitOfWorkMock,
            _loggerMock);
        
        // act
        var result = await service.Handle(command, ct);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Should().HaveCount(1);
        result.Value.Should().Be(volunteer.Pets[0].Id.Value);
    }
    
    [Fact]
    public async Task Add_Pet_To_Volunteer_Should_Be_Failure()
    {
        // arrange
        var ct = new CancellationTokenSource().Token;
        var volunteer = VolunteerFactory.CreateVolunteer();
        var command = VolunteerFactory.CreateAddPetToVolunteerCommand(volunteer.Id.Value);
        
        _volunteersRepositoryMock.GetById(volunteer.Id, ct)
            .Returns(Error.Failure("test.code", "test message"));

        _validatorMock.ValidateAsync(command, ct).Returns(new ValidationResult());
        _unitOfWorkMock.SaveChanges(ct).Returns(Task.CompletedTask);
        _loggerMock.LogInformation("Success");

        var service = new AddPetToVolunteerService(
            _volunteersRepositoryMock,
            _readDbContextMock,
            _validatorMock,
            _unitOfWorkMock,
            _loggerMock);
        
        // act
        var result = await service.Handle(command, ct);
        
        // assert
        result.IsFailure.Should().BeTrue();
        volunteer.Pets.Should().HaveCount(0);
        result.Error.First().Code.Should().Be("test.code");
    }
}