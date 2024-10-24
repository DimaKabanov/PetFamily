using PetFamily.Core.Abstractions.CQRS;
using PetFamily.Core.Dto;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int Experience,
    string Phone,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites) : ICommand;