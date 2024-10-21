using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;

namespace PetFamily.Species.Application.Commands.Breed.DeleteBreed;

public class DeleteBreedService(
    ISpeciesRepository speciesRepository,
    IReadDbContext readDbContext,
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
        
        var petWithDeletingBreed = await readDbContext.Pets
            .FirstOrDefaultAsync(p => p.BreedId == breedId.Value, ct);

        if (petWithDeletingBreed is not null)
            return Errors.General.ValueStillUsing(breedId.Value).ToErrorList();
        
        var speciesResult = await speciesRepository.GetSpeciesById(speciesId, ct);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var breedToDeleteResult = speciesResult.Value.GetBreedById(breedId);
        if (breedToDeleteResult.IsFailure)
            return breedToDeleteResult.Error.ToErrorList();

        speciesResult.Value.DeleteBreed(breedToDeleteResult.Value);

        await unitOfWork.SaveChanges(ct);

        logger.LogInformation("Deleted breed with id {breedId}", breedId);

        return breedId.Value;
    }
}