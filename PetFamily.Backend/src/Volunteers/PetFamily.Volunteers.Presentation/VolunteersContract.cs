using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteers.Presentation;

public class VolunteersContract(IReadDbContext readDbContext) : IVolunteersContract
{
    public async Task<Result<PetDto, Error>> GetPetByBreedId(
        Guid breedId, 
        CancellationToken ct)
    {
       var pet = await readDbContext.Pets.
            FirstOrDefaultAsync(p => p.BreedId == breedId, ct);

       return pet is not null ? pet : Errors.General.NotFound();
    }
    
    public async Task<Result<PetDto, Error>> GetPetBySpeciesId(
        Guid speciesId, 
        CancellationToken ct)
    {
        var pet = await readDbContext.Pets.
            FirstOrDefaultAsync(p => p.SpeciesId == speciesId, ct);

        return pet is not null ? pet : Errors.General.NotFound();
    }
}