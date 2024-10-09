using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteers;

public record GetVolunteersQuery(int Page, int PageSize) : IQuery;