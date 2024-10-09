using PetFamily.Application.Volunteers.Queries.GetVolunteers;

namespace PetFamily.API.Controllers.Volunteer.Requests;

public record GetVolunteersRequest(int Page, int PageSize)
{
    public GetVolunteersQuery ToQuery() => new(Page, PageSize);
}