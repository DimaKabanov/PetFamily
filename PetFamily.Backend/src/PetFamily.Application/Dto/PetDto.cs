using PetFamily.Domain.Enums;

namespace PetFamily.Application.Dto;

public class PetDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public string Color { get; init; } = string.Empty;
    
    public string Health { get; init; } = string.Empty;
    
    public double Weight { get; init; }
    
    public double Height { get; init; }
    
    public string Street { get; init; } = string.Empty;

    public int Home { get; init; }

    public int Flat { get; init; }
    
    public string Phone { get; init; } = string.Empty;
    
    public bool IsCastrated { get; init; }
    
    public DateOnly DateOfBirth { get; init; }
    
    public bool IsVaccinated { get; init; }
    
    public AssistanceStatus AssistanceStatus { get; init; }

    public DateTime CreatedDate { get; init; }
    
    public RequisiteDto[] Requisites { get; init; } = null!;
    
    public PhotoDto[] Photos { get; init; } = null!;
    
    public Guid SpeciesId { get; init; }

    public Guid BreedId { get; init; }

    public int Position { get; init; }
    
    public bool IsDeleted { get; init; }
}