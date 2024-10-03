using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PetFamily.Application.Database;
using PetFamily.Application.PhotoProvider;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.AddPhotoToPet;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.UnitTests.Infrastructure;

namespace PetFamily.Application.UnitTests;

public class UploadPhotosToPetTests
{
    [Fact]
    public async Task Upload_Photos_To_Pet_Should_Be_Success()
    {
        // arrange
        var ct = new CancellationTokenSource().Token;
        
        var volunteer = VolunteerFactory.CreateVolunteerWithPets(1);
        var pet = volunteer.Pets[0];
        var photos = VolunteerFactory.CreatePhotoList(1);
        var paths = VolunteerFactory.CreatePhotoPathList(1);
        
        var command = new UploadPhotoToPetCommand(volunteer.Id.Value, pet.Id.Value, photos);

        var volunteersRepositoryMock = Substitute.For<IVolunteersRepository>();
        volunteersRepositoryMock.GetById(volunteer.Id, ct).Returns(volunteer);

        var photoProviderMock = Substitute.For<IPhotoProvider>();
        photoProviderMock.UploadFiles(Arg.Any<List<PhotoData>>(), ct)
            .Returns(Result.Success<IReadOnlyList<PhotoPath>, Error>(paths));

        var validatorMock = Substitute.For<IValidator<UploadPhotoToPetCommand>>();
        validatorMock.ValidateAsync(command, ct).Returns(new ValidationResult());

        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        unitOfWorkMock.SaveChanges(ct).Returns(Task.CompletedTask);

        var loggerMock = Substitute.For<ILogger<UploadPhotoToPetService>>();
        loggerMock.LogInformation("Success");
        
        var service  = new UploadPhotoToPetService(
            volunteersRepositoryMock,
            photoProviderMock,
            validatorMock,
            unitOfWorkMock,
            loggerMock);
        
        // act
        var result = await service.UploadPhoto(command, ct);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet.Id.Value);
        pet.Photos.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task Upload_Photos_To_Pet_Should_Be_Failure()
    {
        // arrange
        var ct = new CancellationTokenSource().Token;
        
        var volunteer = VolunteerFactory.CreateVolunteerWithPets(1);
        var pet = volunteer.Pets[0];
        var photos = VolunteerFactory.CreatePhotoList(1);
        var paths = VolunteerFactory.CreatePhotoPathList(1);
        
        var command = new UploadPhotoToPetCommand(volunteer.Id.Value, pet.Id.Value, photos);

        var volunteersRepositoryMock = Substitute.For<IVolunteersRepository>();
        volunteersRepositoryMock.GetById(volunteer.Id, ct)
            .Returns(Error.Failure("test.code", "test message"));

        var photoProviderMock = Substitute.For<IPhotoProvider>();
        photoProviderMock.UploadFiles(Arg.Any<List<PhotoData>>(), ct)
            .Returns(Result.Success<IReadOnlyList<PhotoPath>, Error>(paths));

        var validatorMock = Substitute.For<IValidator<UploadPhotoToPetCommand>>();
        validatorMock.ValidateAsync(command, ct).Returns(new ValidationResult());

        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        unitOfWorkMock.SaveChanges(ct).Returns(Task.CompletedTask);

        var loggerMock = Substitute.For<ILogger<UploadPhotoToPetService>>();
        loggerMock.LogInformation("Success");
        
        var service  = new UploadPhotoToPetService(
            volunteersRepositoryMock,
            photoProviderMock,
            validatorMock,
            unitOfWorkMock,
            loggerMock);
        
        // act
        var result = await service.UploadPhoto(command, ct);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().Code.Should().Be("test.code");
        pet.Photos.Should().BeNull();
    }
}