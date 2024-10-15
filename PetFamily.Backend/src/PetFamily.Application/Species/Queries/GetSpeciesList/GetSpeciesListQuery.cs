using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Queries.GetSpeciesList;

public record GetSpeciesListQuery(int Page, int PageSize) : IQuery;