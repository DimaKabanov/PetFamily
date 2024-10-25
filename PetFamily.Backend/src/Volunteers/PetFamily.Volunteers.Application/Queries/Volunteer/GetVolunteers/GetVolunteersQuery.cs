using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteers;

public record GetVolunteersQuery(int Page, int PageSize) : IQuery;