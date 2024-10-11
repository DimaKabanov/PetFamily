using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository(WriteDbContext dbContext) : ISpeciesRepository
{
    public async Task<Result<Species, Error>> GetSpeciesById(SpeciesId speciesId, CancellationToken ct)
    {
        var species = await dbContext.Species
            .FirstOrDefaultAsync(s => s.Id == speciesId, ct);

        if (species is null)
            return Errors.General.NotFound(speciesId.Value);

        return species;
    }
    
    public Guid DeleteSpecies(Species species, CancellationToken ct)
    {
        dbContext.Species.Remove(species);

        return species.Id.Value;
    }
}