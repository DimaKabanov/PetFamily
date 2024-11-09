using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteers;

public class GetVolunteersService(
    IReadDbContext readDbContext,
    IValidator<GetVolunteersQuery> validator) : IQueryService<Result<PagedList<VolunteerDto>, ErrorList>, GetVolunteersQuery>
{
    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
        GetVolunteersQuery query,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(query, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerQuery = readDbContext.Volunteers;
        
        return await volunteerQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}