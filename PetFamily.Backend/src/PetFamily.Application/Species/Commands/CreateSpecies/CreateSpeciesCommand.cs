using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Commands.CreateSpecies;

public record CreateSpeciesCommand(string Name) : ICommand;