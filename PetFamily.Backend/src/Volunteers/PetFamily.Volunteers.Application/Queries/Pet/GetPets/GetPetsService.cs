using System.Linq.Expressions;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;

namespace PetFamily.Volunteers.Application.Queries.Pet.GetPets;

public class GetPetsService(
    IReadDbContext readDbContext) : IQueryService<PagedList<PetDto>, GetPetsQuery>
{
    public async Task<PagedList<PetDto>> Handle(
        GetPetsQuery query,
        CancellationToken ct)
    {
        var petsQuery = readDbContext.Pets;

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.VolunteerId.ToString()),
            p => p.VolunteerId == query.VolunteerId);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Name),
            p => p.Name.Contains(query.Name!));

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Description),
            p => p.Description.Contains(query.Description!));

        petsQuery = SortPets(petsQuery, query.SortBy, query.SortDirection);

        return await petsQuery.ToPagedList(query.Page, query.PageSize, ct);
    }

    private static IQueryable<PetDto> SortPets(IQueryable<PetDto> pets, string? sortBy, string? sortDirection)
    {
        Expression<Func<PetDto, object>> keySelector = sortBy?.ToLower() switch
        {
            "name" => (p) => p.Name,
            "iscastrated" => (p) => p.IsCastrated,
            "position" => (p) => p.Position,
            _ => (p) => p.Id
        };

        return sortDirection?.ToLower() == "desc"
            ? pets.OrderByDescending(keySelector)
            : pets.OrderBy(keySelector);
    }
}