using FluentAssertions;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
using PetFamily.UnitTests.Infrastructure;

namespace PetFamily.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Pet_Should_Be_Success()
    {
        // arrange
        var volunteer = VolunteerFactory.CreateVolunteer();
        var pet =  VolunteerFactory.CreatePet();
        
        // act
        var addedPetResult = volunteer.AddPet(pet);
        var petResult = volunteer.GetPetById(pet.Id);
        
        // assert
        addedPetResult.IsSuccess.Should().BeTrue();
        petResult.Value.Position.Should().Be(Position.First);
    }

    [Fact]
    public void Move_Pet_Should_Not_Move_When_Pet_Already_At_New_Position()
    {
        // arrange
        const int petsCount = 3;
        
        var volunteer =  VolunteerFactory.CreateVolunteerWithPets(petsCount);
        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        
        // act
        var result = volunteer.MovePet(secondPet, secondPosition);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Forward_When_New_Positions_Is_Lower()
    {
        // arrange
        const int petsCount = 5;
        
        var volunteer =  VolunteerFactory.CreateVolunteerWithPets(petsCount);
        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        
        // act
        var result = volunteer.MovePet(fourthPet, secondPosition);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4); 
        fourthPet.Position.Value.Should().Be(2);
        fifthPet.Position.Value.Should().Be(5);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Back_When_New_Positions_Is_Grater()
    {
        // arrange
        const int petsCount = 5;
        
        var volunteer =  VolunteerFactory.CreateVolunteerWithPets(petsCount);
        var fourthPosition = Position.Create(4).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        
        // act
        var result = volunteer.MovePet(secondPet, fourthPosition);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(4);
        thirdPet.Position.Value.Should().Be(2); 
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(5);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Forward_When_New_Positions_Is_First()
    {
        // arrange
        const int petsCount = 3;
        
        var volunteer =  VolunteerFactory.CreateVolunteerWithPets(petsCount);
        var firstPosition = Position.Create(1).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        
        // act
        var result = volunteer.MovePet(thirdPet, firstPosition);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(1); 
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Forward_When_New_Positions_Is_Last()
    {
        // arrange
        const int petsCount = 3;
        
        var volunteer =  VolunteerFactory.CreateVolunteerWithPets(petsCount);
        var thirdPosition = Position.Create(3).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        
        // act
        var result = volunteer.MovePet(firstPet, thirdPosition);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(3);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2); 
    }
}