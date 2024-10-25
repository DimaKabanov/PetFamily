using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteer;

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