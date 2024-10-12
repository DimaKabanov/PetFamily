using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Species.Queries.GetSpeciesList;

public class GetSpeciesListService(
    IReadDbContext readDbContext) : IQueryService<PagedList<SpeciesDto>, GetSpeciesListQuery>
{
    public async Task<PagedList<SpeciesDto>> Handle(
        GetSpeciesListQuery query,
        CancellationToken ct)
    {
        var speciesQuery = readDbContext.Species;
        
        return await speciesQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}