namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    Guid Delete(Domain.Models.Species.Species species, CancellationToken ct);
}