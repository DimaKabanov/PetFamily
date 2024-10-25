using PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteers;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record GetVolunteersRequest(int Page, int PageSize)
{
    public GetVolunteersQuery ToQuery() => new(Page, PageSize);
}