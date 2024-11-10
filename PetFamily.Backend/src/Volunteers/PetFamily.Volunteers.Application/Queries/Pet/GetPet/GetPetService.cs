using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Pet.GetPet;

public class GetPetService(IReadDbContext readDbContext,
    IValidator<GetPetQuery> validator) : IQueryService<Result<PetDto, ErrorList>, GetPetQuery>
{
    public async Task<Result<PetDto, ErrorList>> Handle(GetPetQuery query, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(query, ct);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var pet = await readDbContext.Pets
            .FirstOrDefaultAsync(p => p.Id == query.PetId, ct);

        return pet is not null ? pet : Errors.General.NotFound(query.PetId).ToErrorList();
    }
}