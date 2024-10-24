using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteers;

public class GetVolunteersService(
    IReadDbContext readDbContext) : IQueryService<PagedList<VolunteerDto>, GetVolunteersQuery>
{
    public async Task<PagedList<VolunteerDto>> Handle(
        GetVolunteersQuery query,
        CancellationToken ct)
    {
        var volunteerQuery = readDbContext.Volunteers;
        
        return await volunteerQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}