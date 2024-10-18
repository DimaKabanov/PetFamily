using PetFamily.Application.Volunteers.Queries.GetPets;

namespace PetFamily.API.Controllers.Pets.Requests;

public record GetPetsRequest(
    int Page,
    int PageSize,
    Guid? VolunteerId,
    string? Name,
    string? Description,
    string? SortBy,
    string? SortDirection)
{
    public GetPetsQuery ToQuery() => new(Page, PageSize, VolunteerId, Name, Description, SortBy, SortDirection);
};