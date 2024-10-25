using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure;

public class SpeciesRepository(WriteDbContext dbContext) : ISpeciesRepository
{
    public async Task<Guid> Add(Domain.Species species, CancellationToken ct)
    {
        await dbContext.Species.AddAsync(species, ct);

        return species.Id.Value;
    }
    
    public async Task<Result<Domain.Species, Error>> GetSpeciesById(SpeciesId speciesId, CancellationToken ct)
    {
        var species = await dbContext.Species
            .FirstOrDefaultAsync(s => s.Id == speciesId, ct);

        if (species is null)
            return Errors.General.NotFound(speciesId.Value);

        return species;
    }
    
    public Guid DeleteSpecies(Domain.Species species, CancellationToken ct)
    {
        dbContext.Species.Remove(species);

        return species.Id.Value;
    }
}