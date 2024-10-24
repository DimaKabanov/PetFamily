using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Pet.AddToVolunteer;

public class AddToVolunteerCommandValidator : AbstractValidator<AddToVolunteerCommand>
{
    public AddToVolunteerCommandValidator()
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
            .IsInEnum()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(a => a.CreatedDate).MustBeValueObject(CreatedDate.Create);
        
        RuleForEach(a => a.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}