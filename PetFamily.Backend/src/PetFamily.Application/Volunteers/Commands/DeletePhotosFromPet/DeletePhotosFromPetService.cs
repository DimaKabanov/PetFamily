using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Messaging;
using PetFamily.Application.PhotoProvider;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.DeletePhotosFromPet;

public class DeletePhotosFromPetService(
    IVolunteersRepository volunteersRepository,
    IValidator<DeletePhotosFromPetCommand> validator,
    IUnitOfWork unitOfWork,
    IMessageQueue<IEnumerable<PhotoInfo>> messageQueue,
    ILogger<DeletePhotosFromPetService> logger) : ICommandService<Guid, DeletePhotosFromPetCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePhotosFromPetCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, ct);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();
        
        var photosToDelete= petResult.Value.Photos
            .Select(p => new PhotoInfo(p.Path, Constants.PHOTO_BUCKET_NAME));

        await messageQueue.WriteAsync(photosToDelete, ct);
        
        petResult.Value.DeleteAllPhotos();
        
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Deleted all photos from pet with id: {petId}", petId);

        return petId.Value;
    }
}