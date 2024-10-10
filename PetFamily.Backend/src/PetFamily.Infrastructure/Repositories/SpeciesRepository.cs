using PetFamily.Application.Species;
using PetFamily.Domain.Models.Species;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository(WriteDbContext dbContext) : ISpeciesRepository
{
    public Guid Delete(Species species, CancellationToken ct)
    {
        dbContext.Species.Remove(species);

        return species.Id.Value;
    }
}