using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo;

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