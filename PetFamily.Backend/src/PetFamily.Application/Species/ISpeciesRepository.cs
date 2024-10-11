using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    Task<Result<Domain.Models.Species.Species, Error>> GetSpeciesById(SpeciesId speciesId, CancellationToken ct);
    
    Guid DeleteSpecies(Domain.Models.Species.Species species, CancellationToken ct);
}