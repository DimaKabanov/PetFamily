using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Queries.GetBreedsBySpecies;

public class GetBreedsBySpeciesService(
    IReadDbContext readDbContext) : IQueryService<Result<List<BreedDto>, Error>, GetBreedsBySpeciesQuery>
{
    public async Task<Result<List<BreedDto>, Error>> Handle(
        GetBreedsBySpeciesQuery query,
        CancellationToken ct)
    {
        var species = await readDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == query.SpeciesId, ct);

        if (species is null)
            return Errors.General.NotFound(query.SpeciesId);

        return species.Breeds;
    }
}