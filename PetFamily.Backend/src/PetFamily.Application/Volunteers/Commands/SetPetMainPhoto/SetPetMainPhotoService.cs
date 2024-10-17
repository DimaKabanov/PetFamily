using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.SetPetMainPhoto;

public class SetPetMainPhotoService(
    IVolunteersRepository volunteersRepository,
    IValidator<SetPetMainPhotoCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<SetPetMainPhotoService> logger) : ICommandService<string, SetPetMainPhotoCommand>
{
    public async Task<Result<string, ErrorList>> Handle(
        SetPetMainPhotoCommand command,
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