using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Species.Application.Queries.Species.GetSpecies;

public record GetSpeciesQuery(int Page, int PageSize) : IQuery;