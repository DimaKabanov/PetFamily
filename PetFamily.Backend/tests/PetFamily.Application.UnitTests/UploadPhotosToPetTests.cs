using Bogus;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.PhotoProvider;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.AddPetToVolunteer;
using PetFamily.Application.Volunteers.AddPhotoToPet;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.UnitTests.Infrastructure;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PetFamily.Application.UnitTests;

public class UploadPhotosToPetTests
{
    [Fact]
    public async Task Add_Pet_To_Volunteer_Should_Be_Success()
    {
        var ct = new CancellationTokenSource().Token;

        var volunteer = VolunteerFactory.CreateVolunteer();
        
        var f = new Faker("ru");
        
        var name = f.Lorem.Word();
        var description = f.Lorem.Paragraph();
        
        var physicalProperty = new PetPhysicalPropertyDto(
            f.Commerce.Color(),
            f.Lorem.Word(),
            f.Random.Int(1, 10),
            f.Random.Int(1, 10));
        
        var address = new PetAddressDto(
            f.Address.StreetName(), 
            f.Random.Int(1, 10), 
            f.Random.Int(1, 10));
        
        var phone = f.Phone.PhoneNumber("###########");
        var dateOfBirth = DateOnly.FromDateTime(DateTime.Now);
        var createdDate = DateTime.Now;
        
        var command = new AddPetToVolunteerCommand(
            volunteer.Id.Value,
            name,
            description,
            physicalProperty,
            address,
            phone,
            true,
            dateOfBirth,
            true,
            AssistanceStatus.NeedsHelp,
            createdDate,
            []);
        
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
    }
    
    [Fact]
    public async Task Upload_Photos_To_Pet_Should_Be_Success()
    {
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

        var result = await service.UploadPhoto(command, ct);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet.Id.Value);
        pet.Photos.Count.Should().Be(1);
    }
}