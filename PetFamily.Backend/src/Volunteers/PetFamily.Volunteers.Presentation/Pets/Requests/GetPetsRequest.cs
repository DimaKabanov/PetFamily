using PetFamily.Volunteers.Application.Queries.Pet.GetPets;

namespace PetFamily.Volunteers.Presentation.Pets.Requests;

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