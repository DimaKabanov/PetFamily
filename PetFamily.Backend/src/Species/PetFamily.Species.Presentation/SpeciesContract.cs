using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;
using PetFamily.Species.Application;
using PetFamily.Species.Contracts;

namespace PetFamily.Species.Presentation;

public class SpeciesContract(IReadDbContext readDbContext) : ISpeciesContract
{
    public async Task<Result<SpeciesDto, Error>> GetSpeciesById(
        Guid speciesId,
        CancellationToken ct)
    {
        var species = await readDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == speciesId, ct);

        return species is not null ? species : Errors.General.NotFound();
    }

    public async Task<Result<BreedDto, Error>> GetBreedBySpeciesId(
        Guid speciesId,
        CancellationToken ct)
    {
        var breed = await readDbContext.Breeds
            .FirstOrDefaultAsync(b => b.SpeciesId == speciesId, ct);

        return breed is not null ? breed : Errors.General.NotFound();
    }
}