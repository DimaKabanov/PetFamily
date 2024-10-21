using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.UpdateMainInfo;

public class UpdateVolunteerMainInfoCommandValidator : AbstractValidator<UpdateVolunteerMainInfoCommand>
{
    public UpdateVolunteerMainInfoCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.FullName)
            .MustBeValueObject(f => FullName.Create(f.Name, f.Surname, f.Patronymic));
        
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        
        RuleFor(c => c.Phone).MustBeValueObject(Phone.Create);
    }
}