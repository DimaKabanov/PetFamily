using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteers;

public class GetVolunteersService(
    IReadDbContext readDbContext) : IQueryService<PagedList<VolunteerDto>, GetVolunteersQuery>
{
    public async Task<PagedList<VolunteerDto>> Run(
        GetVolunteersQuery query,
        CancellationToken ct)
    {
        var volunteerQuery = readDbContext.Volunteers;
        
        return await volunteerQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}