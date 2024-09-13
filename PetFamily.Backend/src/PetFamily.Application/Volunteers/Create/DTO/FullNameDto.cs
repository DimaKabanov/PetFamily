namespace PetFamily.Application.Volunteers.Create.DTO;

public record FullNameDto(
    string Name,
    string Surname,
    string? Patronymic);