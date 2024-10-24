using PetFamily.Core.Dto;

namespace PetFamily.Species.Application;

public interface IReadDbContext
{
    IQueryable<SpeciesDto> Species { get; }
    
    IQueryable<BreedDto> Breeds { get; }
}