using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.Breed.GetBreedsBySpecies;

public class GetBreedsBySpeciesService(
    IReadDbContext readDbContext,
    IValidator<GetBreedsBySpeciesQuery> validator) : IQueryService<Result<List<BreedDto>, ErrorList>, GetBreedsBySpeciesQuery>
{
    public async Task<Result<List<BreedDto>, ErrorList>> Handle(
        GetBreedsBySpeciesQuery query,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(query, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var species = await readDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == query.SpeciesId, ct);

        if (species is null)
            return Errors.General.NotFound(query.SpeciesId).ToErrorList();

        return species.Breeds;
    }
}