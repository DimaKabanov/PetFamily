using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.UpdateRequisites;

public class UpdateVolunteerRequisitesCommandValidator : AbstractValidator<UpdateVolunteerRequisitesCommand>
{
    public UpdateVolunteerRequisitesCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(u => u.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}