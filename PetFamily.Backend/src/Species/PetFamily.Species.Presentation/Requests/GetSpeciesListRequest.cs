using PetFamily.Species.Application.Queries.Species.GetSpecies;

namespace PetFamily.Species.Presentation.Requests;

public record GetSpeciesListRequest(int Page, int PageSize)
{
    public GetSpeciesQuery ToQuery() => new(Page, PageSize);
};