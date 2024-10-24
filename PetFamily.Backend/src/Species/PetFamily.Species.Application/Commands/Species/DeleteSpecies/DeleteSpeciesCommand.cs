using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Species.Application.Commands.Species.DeleteSpecies;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;