using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites) : ICommand;