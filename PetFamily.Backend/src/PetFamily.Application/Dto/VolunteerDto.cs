namespace PetFamily.Application.Dto;

public class VolunteerDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string Surname { get; init; } = string.Empty;
    
    public string? Patronymic { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int Experience { get; init; }

    public string Phone { get; init; } = string.Empty;
    
    public RequisiteDto[] Requisites { get; init; } = null!;
    
    public SocialNetworkDto[] SocialNetworks { get; init; } = null!;
}