using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetPets;

public record GetPetsQuery(int Page, int PageSize) : IQuery;