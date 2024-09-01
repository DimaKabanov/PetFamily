namespace PetFamily.Application.Volunteers.CreateVolunteer.DTO;

public record FullNameDto(
    string Name,
    string Surname,
    string? Patronymic
);