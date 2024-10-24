using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteer;

public record GetVolunteerQuery(Guid VolunteerId) : IQuery;