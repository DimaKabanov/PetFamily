using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
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
        var ct = new CancellationTokenSource().Token;

        var volunteer = VolunteerFactory.CreateVolunteer();
        var pet = VolunteerFactory.CreatePet();
        var photos = VolunteerFactory.CreatePhotoList(1);

        volunteer.AddPet(pet);
        
        const string photoName = "photo.png";

        List<PhotoPath> photoPaths = [PhotoPath.Create(photoName).Value];

        var command = new UploadPhotoToPetCommand(volunteer.Id.Value, pet.Id.Value, photos);

        var volunteersRepositoryMock = Substitute.For<IVolunteersRepository>();
        volunteersRepositoryMock.GetById(volunteer.Id, ct).Returns(volunteer);

        var photoProviderMock = Substitute.For<IPhotoProvider>();
        photoProviderMock.UploadFiles(Arg.Any<List<PhotoData>>(), ct)
            .Returns(Result.Success<IReadOnlyList<PhotoPath>, Error>(photoPaths));

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

        var result = await service.UploadPhoto(command, ct);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet.Id.Value);

        pet.Photos.Count.Should().Be(1);
        pet.Photos[0].Path.Path.Should().Be(photoPaths[0].Path);
        pet.Photos[0].IsMain.Should().BeFalse();
    }
}