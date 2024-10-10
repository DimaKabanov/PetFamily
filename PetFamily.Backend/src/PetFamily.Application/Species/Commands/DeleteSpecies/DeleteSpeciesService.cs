using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers.Commands.Delete;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.DeleteSpecies;

public class DeleteSpeciesService(
    ISpeciesRepository speciesRepository,
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
    }
}