using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Species.Breeds;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.DeleteBreed;

public class DeleteBreedService(
    ISpeciesRepository speciesRepository,
    IReadDbContext readDbContext,
    IValidator<DeleteBreedCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<DeleteBreedService> logger) : ICommandService<Guid, DeleteBreedCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var speciesId = SpeciesId.Create(command.SpeciesId);
        var breedId = BreedId.Create(command.BreedId);
        
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