using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetPets;

public class GetPetsService(IReadDbContext readDbContext) : IQueryService<PagedList<PetDto>, GetPetsQuery>
{
    public async Task<PagedList<PetDto>> Handle(GetPetsQuery query, CancellationToken ct)
    {
        var petsQuery = readDbContext.Pets;
        return await petsQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}