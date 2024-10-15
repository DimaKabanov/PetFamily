using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.AddBreedToSpecies;

public record AddBreedToSpeciesCommand(Guid SpeciesId, string Name) : ICommand;