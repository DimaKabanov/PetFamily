using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Species.Application.Queries.Breed.GetBreedsBySpecies;

public record GetBreedsBySpeciesQuery(Guid SpeciesId) : IQuery;