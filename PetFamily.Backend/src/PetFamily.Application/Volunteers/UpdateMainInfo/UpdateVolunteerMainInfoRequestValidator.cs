using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoRequestValidator : AbstractValidator<UpdateVolunteerMainInfoRequest>
{
    public UpdateVolunteerMainInfoRequestValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateVolunteerMainInfoDtoValidator : AbstractValidator<UpdateVolunteerMainInfoDto>
{
    public UpdateVolunteerMainInfoDtoValidator()
    {
        RuleFor(u => u.FullName)
            .MustBeValueObject(f => FullName.Create(f.Name, f.Surname, f.Patronymic));
        
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        
        RuleFor(c => c.Phone).MustBeValueObject(Phone.Create);
    }
}