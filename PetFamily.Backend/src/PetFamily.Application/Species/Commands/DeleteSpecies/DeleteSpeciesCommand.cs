using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.DeleteSpecies;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;