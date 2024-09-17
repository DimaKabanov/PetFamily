using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers.Pets;

public class Pet : Entity<PetId>, ISoftDeletable
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
        DateOnly dateOfBirth,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        DateTime createdDate,
        Detail details,
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
        Details = details;
        Properties = properties;
    }
    
    public Name Name { get; private set; }
    
    public Description Description { get; private set; }
    
    public PhysicalProperty PhysicalProperty { get; private set; }
    
    public Address Address { get; private set; }
    
    public Phone Phone { get; private set; }
    
    public bool IsCastrated { get; private set; }
    
    public DateOnly DateOfBirth { get; private set; }
    
    public bool IsVaccinated { get; private set; }
    
    public AssistanceStatus AssistanceStatus { get; private set; }

    public DateTime CreatedDate { get; private set; }
    
    public Detail Details { get; private set; }
    
    public Property Properties { get; private set; }
    
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
}