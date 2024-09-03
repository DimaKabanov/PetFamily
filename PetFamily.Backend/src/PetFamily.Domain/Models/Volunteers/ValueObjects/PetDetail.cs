using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Models.Volunteers.ValueObjects;

public record PetDetail
{
    public List<PetPhoto> Photos { get; } = [];
    
    public List<Requisite> Requisites { get; } = [];
}