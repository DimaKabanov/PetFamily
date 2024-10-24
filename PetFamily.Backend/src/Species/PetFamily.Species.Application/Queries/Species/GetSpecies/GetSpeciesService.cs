using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Species.Application.Queries.Species.GetSpecies;

public class GetSpeciesService(
    IReadDbContext readDbContext) : IQueryService<PagedList<SpeciesDto>, GetSpeciesQuery>
{
    public async Task<PagedList<SpeciesDto>> Handle(
        GetSpeciesQuery query,
        CancellationToken ct)
    {
        var speciesQuery = readDbContext.Species;
        
        return await speciesQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}