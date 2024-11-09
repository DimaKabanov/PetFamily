using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Species.Application.Commands.Breed.DeleteBreed;

public class DeleteBreedService(
    IVolunteersContract volunteersContract,
    ISpeciesRepository speciesRepository,
    IValidator<DeleteBreedCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<DeleteBreedService> logger) : ICommandService<Guid, DeleteBreedCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedCommand breedCommand,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(breedCommand, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var speciesId = SpeciesId.Create(breedCommand.SpeciesId);
        var breedId = BreedId.Create(breedCommand.BreedId);
        
        var petResult = await volunteersContract.GetPetByBreedId(breedId.Value, ct);
        if (petResult.IsSuccess)
            return Errors.General.ValueStillUsing(breedId.Value).ToErrorList();
        
        var speciesResult = await speciesRepository.GetSpeciesById(speciesId, ct);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var deletingBreedResult = speciesResult.Value.DeleteBreed(breedId);
        if (deletingBreedResult.IsFailure)
            return deletingBreedResult.Error.ToErrorList();

        await unitOfWork.SaveChanges(ct);

        logger.LogInformation("Deleted breed with id {breedId}", breedId);

        return breedId.Value;
    }
}