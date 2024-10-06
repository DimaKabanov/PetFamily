using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PetFamily.Application.Database;
using PetFamily.Application.Messaging;
using PetFamily.Application.PhotoProvider;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.Commands.AddPhotoToPet;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.UnitTests.Infrastructure;

namespace PetFamily.Application.UnitTests;

public class UploadPhotosToPetTests
{
    private readonly IVolunteersRepository _volunteersRepositoryMock = Substitute.For<IVolunteersRepository>();
    private readonly IPhotoProvider _photoProviderMock = Substitute.For<IPhotoProvider>();
    private readonly IValidator<UploadPhotoToPetCommand> _validatorMock = Substitute.For<IValidator<UploadPhotoToPetCommand>>();
    private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    private readonly IMessageQueue<IEnumerable<PhotoInfo>> _messageQueue = Substitute.For<IMessageQueue<IEnumerable<PhotoInfo>>>();
    private readonly ILogger<UploadPhotoToPetService> _loggerMock = Substitute.For<ILogger<UploadPhotoToPetService>>();
    
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
        
        _volunteersRepositoryMock.GetById(volunteer.Id, ct).Returns(volunteer);
        _photoProviderMock.UploadFiles(Arg.Any<List<PhotoData>>(), ct)
            .Returns(Result.Success<IReadOnlyList<PhotoPath>, Error>(paths));
        _validatorMock.ValidateAsync(command, ct).Returns(new ValidationResult());
        _unitOfWorkMock.SaveChanges(ct).Returns(Task.CompletedTask);
        _messageQueue.WriteAsync(Arg.Any<List<PhotoInfo>>(), ct).Returns(Task.CompletedTask);
        _loggerMock.LogInformation("Success");
        
        var service  = new UploadPhotoToPetService(
            _volunteersRepositoryMock,
            _photoProviderMock,
            _validatorMock,
            _unitOfWorkMock,
            _messageQueue,
            _loggerMock);
        
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
        
        _volunteersRepositoryMock.GetById(volunteer.Id, ct)
            .Returns(Error.Failure("test.code", "test message"));
        
        _photoProviderMock.UploadFiles(Arg.Any<List<PhotoData>>(), ct)
            .Returns(Result.Success<IReadOnlyList<PhotoPath>, Error>(paths));
        
        _validatorMock.ValidateAsync(command, ct).Returns(new ValidationResult());
        _unitOfWorkMock.SaveChanges(ct).Returns(Task.CompletedTask);
        _messageQueue.WriteAsync(Arg.Any<List<PhotoInfo>>(), ct).Returns(Task.CompletedTask);
        _loggerMock.LogInformation("Success");
        
        var service  = new UploadPhotoToPetService(
            _volunteersRepositoryMock,
            _photoProviderMock,
            _validatorMock,
            _unitOfWorkMock,
            _messageQueue,
            _loggerMock);
        
        // act
        var result = await service.UploadPhoto(command, ct);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().Code.Should().Be("test.code");
        pet.Photos.Should().BeNull();
    }
}