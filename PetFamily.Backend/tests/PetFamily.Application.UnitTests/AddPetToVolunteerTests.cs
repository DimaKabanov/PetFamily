using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PetFamily.SharedKernel;
using PetFamily.UnitTests.Infrastructure;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Application.Commands.Pet.AddToVolunteer;

namespace PetFamily.Application.UnitTests;

public class AddPetToVolunteerTests
{
    private readonly IVolunteersRepository _volunteersRepositoryMock = Substitute.For<IVolunteersRepository>();
    private readonly IReadDbContext _readDbContextMock = Substitute.For<IReadDbContext>();
    private readonly IValidator<AddToVolunteerCommand> _validatorMock =  Substitute.For<IValidator<AddToVolunteerCommand>>();
    private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    private readonly ILogger<AddToVolunteerService> _loggerMock = Substitute.For<ILogger<AddToVolunteerService>>();
    
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
        
        var service  = new AddToVolunteerService(
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

        var service = new AddToVolunteerService(
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