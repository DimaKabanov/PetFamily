using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.Commands.AddPetToVolunteer;

public class AddPetToVolunteerCommandValidator : AbstractValidator<AddPetToVolunteerCommand>
{
    public AddPetToVolunteerCommandValidator()
    {
        RuleFor(a => a.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(a => a.Name).MustBeValueObject(Name.Create);
        
        RuleFor(a => a.Description).MustBeValueObject(Description.Create);
        
        RuleFor(a => a.PhysicalProperty).MustBeValueObject(
            p => PhysicalProperty.Create(p.Color, p.Health, p.Weight, p.Height));
        
        RuleFor(a => a.Address).MustBeValueObject(
            a => Address.Create(a.Street, a.Home, a.Flat));
        
        RuleFor(a => a.Phone).MustBeValueObject(Phone.Create);
        
        RuleFor(a => a.IsCastrated)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(a => a.DateOfBirth).MustBeValueObject(DateOfBirth.Create);
        
        RuleFor(a => a.IsVaccinated)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(a => a.AssistanceStatus)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(a => a.CreatedDate).MustBeValueObject(CreatedDate.Create);
        
        RuleForEach(a => a.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}