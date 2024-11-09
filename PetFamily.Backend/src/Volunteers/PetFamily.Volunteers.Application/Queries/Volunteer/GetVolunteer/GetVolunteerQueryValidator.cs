using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Volunteer.GetVolunteer;

public class GetVolunteerQueryValidator : AbstractValidator<GetVolunteerQuery>
{
    public GetVolunteerQueryValidator()
    {
        RuleFor(g => g.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}