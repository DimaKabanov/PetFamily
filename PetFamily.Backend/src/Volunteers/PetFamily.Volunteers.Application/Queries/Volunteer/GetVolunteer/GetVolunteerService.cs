using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteer;

public class GetVolunteerService(
    IReadDbContext readDbContext,
    IValidator<GetVolunteerQuery> validator) : IQueryService<Result<VolunteerDto, ErrorList>, GetVolunteerQuery>
{
    public async Task<Result<VolunteerDto, ErrorList>> Handle(
        GetVolunteerQuery query,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(query, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteer = await readDbContext.Volunteers
            .FirstOrDefaultAsync(v => v.Id == query.VolunteerId, ct);

        return volunteer is null
            ? Errors.General.NotFound(query.VolunteerId).ToErrorList()
            : volunteer;
    }
}