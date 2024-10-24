using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Species.Application.Commands.Breed.AddToSpecies;

public record AddToSpeciesCommand(Guid SpeciesId, string Name) : ICommand;