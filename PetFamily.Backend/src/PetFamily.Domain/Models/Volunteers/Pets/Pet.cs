using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers.Pets;

public class Pet : Shared.Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
    private Pet(PetId id) : base(id)
    {
    }
    
    public Pet(
        PetId id,
        Name name,
        Description description,
        PhysicalProperty physicalProperty,
        Address address,
        Phone phone,
        bool isCastrated,
        DateOfBirth dateOfBirth,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        CreatedDate createdDate,
        IReadOnlyList<Requisite> requisites,
        Property properties) : base(id)
    {
        Name = name;
        Description = description;
        PhysicalProperty = physicalProperty;
        Address = address;
        Phone = phone;
        IsCastrated = isCastrated;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        AssistanceStatus = assistanceStatus;
        CreatedDate = createdDate;
        Requisites = requisites;
        Properties = properties;
    }
    
    public Name Name { get; private set; }
    
    public Description Description { get; private set; }
    
    public PhysicalProperty PhysicalProperty { get; private set; }
    
    public Address Address { get; private set; }
    
    public Phone Phone { get; private set; }
    
    public bool IsCastrated { get; private set; }
    
    public DateOfBirth DateOfBirth { get; private set; }
    
    public bool IsVaccinated { get; private set; }
    
    public AssistanceStatus AssistanceStatus { get; private set; }

    public CreatedDate CreatedDate { get; private set; }
    
    public IReadOnlyList<Requisite> Requisites { get; private set; }

    public IReadOnlyList<Photo> Photos { get; private set; } = [];
    
    public Property Properties { get; private set; }

    public Position Position { get; private set; }

    public void UpdatePet(
        Name name,
        Description description,
        PhysicalProperty physicalProperty,
        Address address,
        Phone phone,
        bool isCastrated,
        DateOfBirth dateOfBirth,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        CreatedDate createdDate,
        IReadOnlyList<Requisite> requisites,
        Property properties)
    {
        Name = name;
        Description = description;
        PhysicalProperty = physicalProperty;
        Address = address;
        Phone = phone;
        IsCastrated = isCastrated;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        AssistanceStatus = assistanceStatus;
        CreatedDate = createdDate;
        Requisites = requisites;
        Properties = properties;
    }
    
    public void UpdatePhotos(IReadOnlyList<Photo> photos) =>
        Photos = photos;

    public void SetPosition(Position position) => 
        Position = position;
    
    public void Delete()
    {
        if (!_isDeleted)
            _isDeleted = true;
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }
    
    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }
}