using CSharpFunctionalExtensions;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Contracts;

public interface IVolunteersContract
{
    Task<Result<PetDto, Error>> GetPetByBreedId(Guid breedId, CancellationToken ct);
    
    Task<Result<PetDto, Error>> GetPetBySpeciesId(Guid speciesId, CancellationToken ct);
}