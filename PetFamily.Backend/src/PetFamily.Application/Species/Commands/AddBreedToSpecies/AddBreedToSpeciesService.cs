using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Species.Breeds;
using PetFamily.Domain.Models.Species.Breeds.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.AddBreedToSpecies;

public class AddBreedToSpeciesService(
    ISpeciesRepository speciesRepository,
    IReadDbContext readDbContext,
    IValidator<AddBreedToSpeciesCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<AddBreedToSpeciesService> logger) : ICommandService<Guid, AddBreedToSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        AddBreedToSpeciesCommand command,
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

        var breed = new Breed(breedId, name);
        
        speciesResult.Value.AddBreed(breed);
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Added breed with id: {breedId}", breedId);

        return breedId.Value;
    }
}