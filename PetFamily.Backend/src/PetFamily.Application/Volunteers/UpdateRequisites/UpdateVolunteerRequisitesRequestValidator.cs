using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public class UpdateVolunteerRequisitesRequestValidator : AbstractValidator<UpdateVolunteerRequisitesRequest>
{
    public UpdateVolunteerRequisitesRequestValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateVolunteerRequisitesDtoValidator : AbstractValidator<UpdateVolunteerRequisitesDto>
{
    public UpdateVolunteerRequisitesDtoValidator()
    {
        RuleForEach(u => u.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}