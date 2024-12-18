﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Species.Application.Commands.Species.DeleteSpecies;

public class DeleteSpeciesService(
    IVolunteersContract volunteersContract,
    ISpeciesRepository speciesRepository,
    IValidator<DeleteSpeciesCommand> validator,
    IUnitOfWork unitOfWork,
    ILogger<DeleteSpeciesService> logger) : ICommandService<Guid, DeleteSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesCommand speciesCommand,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(speciesCommand, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var speciesId = SpeciesId.Create(speciesCommand.SpeciesId);

        var petWithDeletingSpeciesResult = await volunteersContract.GetPetBySpeciesId(speciesId.Value, ct);
        if (petWithDeletingSpeciesResult.IsSuccess)
            return Errors.General.ValueStillUsing(speciesCommand.SpeciesId).ToErrorList();

        var speciesResult = await speciesRepository.GetSpeciesById(speciesId, ct);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        speciesRepository.DeleteSpecies(speciesResult.Value, ct);

        await unitOfWork.SaveChanges(ct);
        
        logger.LogInformation("Deleted species with id {speciesId}", speciesId);

        return speciesId.Value;
    }
}