using Bogus;
using FluentAssertions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Pet_Return_Success()
    {
        var volunteer = CreateVolunteer();
        var pet = CreatePet();

        var result = volunteer.AddPet(pet);
        var addedPetResult = volunteer.GetPetById(pet.Id);

        result.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Position.Should().Be(Position.First);
    }

    private static Volunteer CreateVolunteer()
    {
        var f = new Faker("ru");
        
        var fullName = FullName.Create(f.Person.FirstName, f.Person.LastName, f.Person.LastName).Value;
        var description = Description.Create(f.Lorem.Paragraph()).Value;
        var experience = Experience.Create(f.Random.Int(1, 10)).Value;
        var phone = Phone.Create(f.Phone.PhoneNumber("###########")).Value;
        var socialNetworks = new ValueObjectList<SocialNetwork>([]);
        var requisites = new ValueObjectList<Requisite>([]);
        
        return new Volunteer(
            VolunteerId.NewId(),
            fullName,
            description,
            experience,
            phone,
            socialNetworks,
            requisites);
    }

    private Pet CreatePet()
    {
        var f = new Faker("ru");
        
        var name = Name.Create(f.Lorem.Word()).Value;
        var description = Description.Create(f.Lorem.Paragraph()).Value;
        
        var physicalProperty = PhysicalProperty.Create(
            f.Commerce.Color(),
            f.Lorem.Word(),
            f.Random.Int(1, 10),
            f.Random.Int(1, 10)).Value;
        
        var address = Address.Create(
            f.Address.StreetName(), 
            f.Random.Int(1, 10), 
            f.Random.Int(1, 10)).Value;
        
        var phone = Phone.Create(f.Phone.PhoneNumber("###########")).Value;
        var dateOfBirth = DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Now)).Value;
        var createdDate = CreatedDate.Create(DateTime.Now).Value;
        var requisites = new ValueObjectList<Requisite>([]);
        var properties = new Property(SpeciesId.EmptyId, Guid.Empty);
        
        return new Pet(
            PetId.NewId(),
            name,
            description,
            physicalProperty,
            address,
            phone,
            true,
            dateOfBirth,
            true,
            AssistanceStatus.NeedsHelp,
            createdDate,
            requisites,
            properties);
    }
}