using Bogus;
using PetFamily.Application.Dto;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.UnitTests.Infrastructure;

public static class VolunteerFactory
{
    public static Volunteer CreateVolunteerWithPets(int petsCount)
    {
        var volunteer = CreateVolunteer();

        for (var i = 0; i < petsCount; i++)
        {
            var pet = CreatePet();
            volunteer.AddPet(pet);
        }

        return volunteer;
    }

    public static Volunteer CreateVolunteer()
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

    public static Pet CreatePet()
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

    public static List<UploadPhotoDto> CreatePhotoList(int photosCount)
    {
        var f = new Faker("ru");
        var stream = new MemoryStream();
        
        List<UploadPhotoDto> photos = [];
        for (var i = 0; i < photosCount; i++)
        {
            var photoName = $"{f.Lorem.Word()}.png";
            var photo = new UploadPhotoDto(stream, photoName);
            photos.Add(photo);
        }
        
        return photos;
    }
    
    public static List<PhotoPath> CreatePhotoPathList(int pathsCount)
    {
        var f = new Faker("ru");
        
        List<PhotoPath> paths = [];
        for (var i = 0; i < pathsCount; i++)
        {
            var photoName = $"{f.Lorem.Word()}.png";
            var path = PhotoPath.Create(photoName).Value;
            paths.Add(path);
        }
        
        return paths;
    }
}