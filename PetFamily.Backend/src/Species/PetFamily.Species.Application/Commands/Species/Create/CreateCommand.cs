using PetFamily.Core.Abstractions.CQRS;

namespace PetFamily.Species.Application.Commands.Species.Create;

public record CreateCommand(string Name) : ICommand;