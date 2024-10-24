using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Queries.Pet.GetPet;

public record GetPetQuery(Guid PetId) : IQuery;