using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Species.Application.Commands.Breed.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;