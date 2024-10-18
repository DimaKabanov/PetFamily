using PetFamily.Application.Volunteers.Queries.GetPets;

namespace PetFamily.API.Controllers.Pets.Requests;

public record GetPetsRequest(int Page, int PageSize)
{
    public GetPetsQuery ToQuery() => new(Page, PageSize);
};