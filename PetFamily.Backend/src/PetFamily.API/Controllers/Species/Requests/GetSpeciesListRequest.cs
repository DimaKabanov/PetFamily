using PetFamily.Application.Species.Queries.GetSpeciesList;

namespace PetFamily.API.Controllers.Species.Requests;

public record GetSpeciesListRequest(int Page, int PageSize)
{
    public GetSpeciesListQuery ToQuery() => new(Page, PageSize);
};