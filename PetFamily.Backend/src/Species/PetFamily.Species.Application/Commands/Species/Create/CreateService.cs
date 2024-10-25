using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.Species.Create;

public class CreateService(
    ISpeciesRepository speciesRepository,
    IReadDbContext readDbContext,
    IValidator<CreateCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<CreateService> logger) : ICommandService<Guid, CreateCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateCommand command,
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

        var species = new Domain.Species(speciesId, name);

        await speciesRepository.Add(species, ct);
        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Create species with id: {speciesId}", speciesId);

        return speciesId.Value;
    }
}