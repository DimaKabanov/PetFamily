using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Species.Domain.Breeds.ValueObjects;

namespace PetFamily.Species.Application.Commands.Breed.AddToSpecies;

public class AddToSpeciesService(
    ISpeciesRepository speciesRepository,
    IReadDbContext readDbContext,
    IValidator<AddToSpeciesCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<AddToSpeciesService> logger) : ICommandService<Guid, AddToSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        AddToSpeciesCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var speciesId = SpeciesId.Create(command.SpeciesId);
        
        var speciesResult = await speciesRepository.GetSpeciesById(speciesId, ct);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();
        
        var name = Name.Create(command.Name).Value;
        
        var existingBreed = speciesResult.Value.GetBreedByName(name);
        if (existingBreed.IsSuccess)
            return Errors.General.ValueAlreadyExisting(existingBreed.Value.Id.Value).ToErrorList();

        var breedId = BreedId.NewId();

        var breed = new Domain.Breeds.Breed(breedId, name);
        
        speciesResult.Value.AddBreed(breed);
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Added breed with id: {breedId}", breedId);

        return breedId.Value;
    }
}