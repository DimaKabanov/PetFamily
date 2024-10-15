using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Species.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.CreateSpecies;

public class CreateSpeciesService(
    ISpeciesRepository speciesRepository,
    IReadDbContext readDbContext,
    IValidator<CreateSpeciesCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateSpeciesService> logger) : ICommandService<Guid, CreateSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpeciesCommand command,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var alreadyExistingSpecies = await readDbContext.Species
            .FirstOrDefaultAsync(s => s.Name == command.Name, ct);

        if (alreadyExistingSpecies is not null)
            return Errors.General.ValueAlreadyExisting(alreadyExistingSpecies.Id).ToErrorList();

        var speciesId = SpeciesId.NewId();

        var name = Name.Create(command.Name).Value;

        var species = new Domain.Models.Species.Species(speciesId, name);

        await speciesRepository.Add(species, ct);
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Create species with id: {speciesId}", speciesId);

        return speciesId.Value;
    }
}