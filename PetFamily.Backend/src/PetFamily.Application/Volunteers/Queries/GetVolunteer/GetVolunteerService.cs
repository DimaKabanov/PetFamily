using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteer;

public class GetVolunteerService(
    IReadDbContext readDbContext) : IQueryService<Result<VolunteerDto, Error>, GetVolunteerQuery>
{
    public async Task<Result<VolunteerDto, Error>> Handle(
        GetVolunteerQuery query,
        CancellationToken ct)
    {
        var volunteer = await readDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == query.VolunteerId, ct);

        return volunteer is null
            ? Errors.General.NotFound(query.VolunteerId)
            : volunteer;
    }
}