using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateRequisites;

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