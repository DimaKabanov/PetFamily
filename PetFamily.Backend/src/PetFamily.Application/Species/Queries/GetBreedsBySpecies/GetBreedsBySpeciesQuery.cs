using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Queries.GetBreedsBySpecies;

public record GetBreedsBySpeciesQuery(Guid SpeciesId) : IQuery;