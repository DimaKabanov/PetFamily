using CSharpFunctionalExtensions;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Contracts;

public interface ISpeciesContract
{
    Task<Result<SpeciesDto, Error>> GetSpeciesById(Guid speciesId, CancellationToken ct);

    Task<Result<BreedDto, Error>> GetBreedBySpeciesId(Guid speciesId, CancellationToken ct);
}