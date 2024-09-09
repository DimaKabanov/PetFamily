using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(f => FullName.Create(f.Name, f.Surname, f.Patronymic));
        
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        
        RuleFor(c => c.Phone).MustBeValueObject(Phone.Create);

        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(s => SocialNetwork.Create(s.Title, s.Url));
        
        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}