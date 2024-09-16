namespace PetFamily.Application.Volunteers.DTO;

public record FullNameDto(
    string Name,
    string Surname,
    string? Patronymic);