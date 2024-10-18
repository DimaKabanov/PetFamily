using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetPets;

public record GetPetsQuery(
    int Page,
    int PageSize,
    Guid? VolunteerId,
    string? Name,
    string? Description,
    string? SortBy,
    string? SortDirection) : IQuery;