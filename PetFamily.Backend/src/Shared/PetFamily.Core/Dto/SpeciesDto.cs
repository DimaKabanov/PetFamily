namespace PetFamily.Core.Dto;

public class SpeciesDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public List<BreedDto> Breeds { get; init; } = default!;
}