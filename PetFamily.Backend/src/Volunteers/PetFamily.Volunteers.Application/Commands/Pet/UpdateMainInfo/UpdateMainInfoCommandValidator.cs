using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Volunteers.Application.Commands.Pet.UpdateMainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainIngoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(e => e.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(e => e.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(e => e.SpeciesId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(e => e.BreedId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(e => e.Name).MustBeValueObject(Name.Create);
        
        RuleFor(e => e.Description).MustBeValueObject(Description.Create);
        
        RuleFor(e => e.PhysicalProperty).MustBeValueObject(
            p => PhysicalProperty.Create(p.Color, p.Health, p.Weight, p.Height));
        
        RuleFor(e => e.Address).MustBeValueObject(
            a => Address.Create(a.Street, a.Home, a.Flat));
        
        RuleFor(e => e.Phone).MustBeValueObject(Phone.Create);
        
        RuleFor(e => e.IsCastrated)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(e => e.DateOfBirth).MustBeValueObject(DateOfBirth.Create);
        
        RuleFor(e => e.IsVaccinated)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(e => e.AssistanceStatus)
            .NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(e => e.CreatedDate).MustBeValueObject(CreatedDate.Create);
        
        RuleForEach(e => e.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}