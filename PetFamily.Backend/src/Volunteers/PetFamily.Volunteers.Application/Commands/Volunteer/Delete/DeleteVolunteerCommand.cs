using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Volunteers.Application.Commands.Volunteer.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;