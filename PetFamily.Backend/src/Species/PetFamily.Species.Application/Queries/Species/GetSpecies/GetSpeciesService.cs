using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.Species.GetSpecies;

public class GetSpeciesService(
    IReadDbContext readDbContext,
    IValidator<GetSpeciesQuery> validator) : IQueryService<Result<PagedList<SpeciesDto>, ErrorList>, GetSpeciesQuery>
{
    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Handle(
        GetSpeciesQuery query,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(query, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var speciesQuery = readDbContext.Species;
        
        return await speciesQuery.ToPagedList(query.Page, query.PageSize, ct);
    }
}