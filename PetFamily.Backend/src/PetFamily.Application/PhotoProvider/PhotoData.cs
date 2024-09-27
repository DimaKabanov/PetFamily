using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

namespace PetFamily.Application.PhotoProvider;

public record PhotoData(Stream Stream, PhotoPath PhotoPath, string BucketName);