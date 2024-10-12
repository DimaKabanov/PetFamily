using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.DeleteSpecies;

public class DeleteSpeciesService(
    ISpeciesRepository speciesRepository,
    IReadDbContext readDbContext,
    IValidator<DeleteSpeciesCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<DeleteSpeciesService> logger) : ICommandService<Guid, DeleteSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var speciesId = SpeciesId.Create(command.SpeciesId);
        
        var petWithDeletingSpecies = await readDbContext.Pets
            .FirstOrDefaultAsync(p => p.SpeciesId == speciesId.Value, ct);
        
        if (petWithDeletingSpecies is not null)
            return Errors.General.ValueStillUsing(command.SpeciesId).ToErrorList();

        var speciesResult = await speciesRepository.GetSpeciesById(speciesId, ct);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        speciesRepository.DeleteSpecies(speciesResult.Value, ct);

        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Deleted species with id {speciesId}", speciesId);

        return speciesId.Value;
    }
}