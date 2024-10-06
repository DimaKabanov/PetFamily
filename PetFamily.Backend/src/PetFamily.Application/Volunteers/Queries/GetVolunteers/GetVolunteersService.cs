using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteers;

public class GetVolunteersService(IReadDbContext readDbContext)
{
    public async Task<PagedList<VolunteerDto>> GetVolunteers(
        GetVolunteersQuery query,
        CancellationToken ct)
    {
        var volunteerQuery = readDbContext.Volunteers;
        
        return await volunteerQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}