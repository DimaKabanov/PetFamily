using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Queries.Pet.GetPets;

public record GetPetsQuery(
    int Page,
    int PageSize,
    Guid? VolunteerId,
    string? Name,
    string? Description,
    string? SortBy,
    string? SortDirection) : IQuery;