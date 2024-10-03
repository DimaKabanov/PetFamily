using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.AddPetToVolunteer;
using PetFamily.Domain.Shared;
using PetFamily.UnitTests.Infrastructure;

namespace PetFamily.Application.UnitTests;

public class AddPetToVolunteerTests
{
    [Fact]
    public async Task Add_Pet_To_Volunteer_Should_Be_Success()
    {
        // arrange
        var ct = new CancellationTokenSource().Token;
        var volunteer = VolunteerFactory.CreateVolunteer();
        var command = VolunteerFactory.CreateAddPetToVolunteerCommand(volunteer.Id.Value);
        
        var volunteersRepositoryMock = Substitute.For<IVolunteersRepository>();
        volunteersRepositoryMock.GetById(volunteer.Id, ct).Returns(volunteer);
        
        var validatorMock = Substitute.For<IValidator<AddPetToVolunteerCommand>>();
        validatorMock.ValidateAsync(command, ct).Returns(new ValidationResult());
        
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        unitOfWorkMock.SaveChanges(ct).Returns(Task.CompletedTask);
        
        var loggerMock = Substitute.For<ILogger<AddPetToVolunteerService>>();
        loggerMock.LogInformation("Success");
        
        var service  = new AddPetToVolunteerService(
            volunteersRepositoryMock,
            validatorMock,
            unitOfWorkMock,
            loggerMock);
        
        // act
        var result = await service.AddPet(command, ct);
        
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

        var volunteersRepositoryMock = Substitute.For<IVolunteersRepository>();
        volunteersRepositoryMock.GetById(volunteer.Id, ct)
            .Returns(Error.Failure("test.code", "test message"));

        var validatorMock = Substitute.For<IValidator<AddPetToVolunteerCommand>>();
        validatorMock.ValidateAsync(command, ct).Returns(new ValidationResult());

        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        unitOfWorkMock.SaveChanges(ct).Returns(Task.CompletedTask);

        var loggerMock = Substitute.For<ILogger<AddPetToVolunteerService>>();
        loggerMock.LogInformation("Success");

        var service = new AddPetToVolunteerService(
            volunteersRepositoryMock,
            validatorMock,
            unitOfWorkMock,
            loggerMock);
        
        // act
        var result = await service.AddPet(command, ct);
        
        // assert
        result.IsFailure.Should().BeTrue();
        volunteer.Pets.Should().HaveCount(0);
        result.Error.First().Code.Should().Be("test.code");
    }
}