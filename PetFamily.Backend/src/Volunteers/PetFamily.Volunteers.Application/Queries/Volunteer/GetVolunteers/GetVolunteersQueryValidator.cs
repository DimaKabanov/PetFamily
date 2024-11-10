using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteers;

public class GetVolunteersQueryValidator: AbstractValidator<GetVolunteersQuery>
{
    public GetVolunteersQueryValidator()
    {
        RuleFor(g => g.Page).GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(g => g.PageSize).GreaterThan(0).WithError(Errors.General.ValueIsInvalid());
    }
}