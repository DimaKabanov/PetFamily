using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;

namespace PetFamily.Volunteers.Application.Queries.Pet.GetPet;

public class GetPetService(IReadDbContext readDbContext) : IQueryService<Result<PetDto, Error>, GetPetQuery>
{
    public async Task<Result<PetDto, Error>> Handle(GetPetQuery query, CancellationToken ct)
    {
        var pet = await readDbContext.Pets
            .FirstOrDefaultAsync(p => p.Id == query.PetId, ct);

        return pet is not null ? pet : Errors.General.NotFound(query.PetId);
    }
}