using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Pet.GetPets;

public class GetPetsService(
    IReadDbContext readDbContext,
    IValidator<GetPetsQuery> validator) : IQueryService<Result<PagedList<PetDto>, ErrorList>, GetPetsQuery>
{
    public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(
        GetPetsQuery query,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(query, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
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