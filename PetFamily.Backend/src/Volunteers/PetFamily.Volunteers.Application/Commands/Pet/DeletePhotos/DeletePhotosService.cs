using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.PhotoProvider;
using PetFamily.SharedKernel.ValueObjects.EntityIds;

namespace PetFamily.Volunteers.Application.Commands.Pet.DeletePhotos;

public class DeletePhotosService(
    IVolunteersRepository volunteersRepository,
    IValidator<DeletePhotosCommand> validator,
    IUnitOfWork unitOfWork,
    IMessageQueue<IEnumerable<PhotoInfo>> messageQueue,
    ILogger<DeletePhotosService> logger) : ICommandService<Guid, DeletePhotosCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePhotosCommand command,
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