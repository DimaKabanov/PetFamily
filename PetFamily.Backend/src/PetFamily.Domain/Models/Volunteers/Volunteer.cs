using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers;

public class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
{
    private readonly List<Pet> _pets = [];
    
    private bool _isDeleted = false;
    
    private Volunteer(VolunteerId id) : base(id)
    {
    }
    
    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description description,
        Experience experience,
        Phone phone,
        ValueObjectList<SocialNetwork> socialNetworks,
        ValueObjectList<Requisite> requisites) : base(id)
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        Phone = phone;
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }
    
    public FullName FullName { get; private set; }

    public Description Description { get; private set; }

    public Experience Experience { get; private set; }
    
    public Phone Phone { get; private set; }

    public ValueObjectList<Requisite> Requisites { get; private set; }
    
    public ValueObjectList<SocialNetwork> SocialNetworks { get; private set; }
    
    public IReadOnlyList<Pet> Pets => _pets;
    
    public int PetsNeedsHelpCount() => _pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedsHelp);
    
    public int PetsSearchHomeCount() => _pets.Count(p => p.AssistanceStatus == AssistanceStatus.SearchAHome);
    
    public int PetsFoundHomeCount() => _pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundAHome);
    
    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        return pet;
    }

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        Experience experience,
        Phone phone)
    {
        FullName = fullName;
        Description = description;
        Experience = experience;
        Phone = phone;
    }
    
    public void UpdateSocialNetworks(ValueObjectList<SocialNetwork> socialNetworks) =>
        SocialNetworks = socialNetworks;
    
    public void UpdateRequisites(ValueObjectList<Requisite> requisites) =>
        Requisites = requisites;

    public void Delete()
    {
        if (_isDeleted) return;
        
        _isDeleted = true;
        foreach (var pet in _pets)
            pet.Delete();
    }

    public void Restore()
    {
        if (!_isDeleted) return;
        
        _isDeleted = false;
        foreach (var pet in _pets)
            pet.Restore();
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var positionResult = Position.Create(_pets.Count + 1);
        if (positionResult.IsFailure)
            return positionResult.Error;
        
        pet.SetPosition(positionResult.Value);
        
        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;

        if (currentPosition == newPosition)
            return Result.Success<Error>();

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;

        newPosition = adjustedPosition.Value;

        var moveResult = MovePetsBetweenPositions(newPosition, currentPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;
        
        pet.SetPosition(newPosition);

        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= _pets.Count)
            return newPosition;
        
        var lastPosition = Position.Create(_pets.Count - 1);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition.Value < currentPosition.Value)
        {
            var petsToMove = _pets.Where(p =>
                p.Position.Value >= newPosition.Value && p.Position.Value < currentPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var moveResult = petToMove.MoveForward();
                if (moveResult.IsFailure)
                    return moveResult.Error;
            }
        }
        else if (newPosition.Value > currentPosition.Value)
        {
            var petsToMove = _pets.Where(p =>
                p.Position.Value > currentPosition.Value && p.Position.Value <= newPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var moveResult = petToMove.MoveBack();
                if (moveResult.IsFailure)
                    return moveResult.Error;
            }
        }
        
        return Result.Success<Error>();
    }
}