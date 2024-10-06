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
    
    public string Requisites { get; init; } = string.Empty;
    
    public string SocialNetworks { get; init; } = string.Empty;
}