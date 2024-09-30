using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Pet_Return_Success()
    {
        var id = VolunteerId.NewId();
        var fullName = FullName.Create("test", "test", "test").Value;
        var description = Description.Create("test").Value;
        var experience = Experience.Create(10).Value;
        var phone = Phone.Create("89527364891").Value;
        var socialNetworks = SocialNetwork.Create("test", "test").Value;
        var requisites = Requisite.Create("test", "test").Value;
        
        var volunteer = new Volunteer(
            id,
            fullName,
            description,
            experience,
            phone,
            socialNetworks,
            requisites);
    }
}