using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.EntityIds;

namespace PetFamily.Volunteers.Application.Commands.Pet.SetMainPhoto;

public class SetMainPhotoService(
    IVolunteersRepository volunteersRepository,
    IValidator<SetMainPhotoCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<SetMainPhotoService> logger) : ICommandService<string, SetMainPhotoCommand>
{
    public async Task<Result<string, ErrorList>> Handle(
        SetMainPhotoCommand command,
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

        var mainPhotoPath = PhotoPath.Create(command.PhotoPath);

        var photoResult = petResult.Value.GetPetPhotoByPath(mainPhotoPath.Value);
        if (photoResult.IsFailure)
            return photoResult.Error.ToErrorList();
        
        petResult.Value.UpdateMainPhoto(mainPhotoPath.Value);
        
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Set is main photo with path: {path} ", mainPhotoPath);
        
        return mainPhotoPath.Value.Path;
    }
}