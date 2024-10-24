using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;

namespace PetFamily.Species.Application;

public interface ISpeciesRepository
{
    Task<Guid> Add(Domain.Species species, CancellationToken ct);
    
    Task<Result<Domain.Species, Error>> GetSpeciesById(SpeciesId speciesId, CancellationToken ct);
    
    Guid DeleteSpecies(Domain.Species species, CancellationToken ct);
}