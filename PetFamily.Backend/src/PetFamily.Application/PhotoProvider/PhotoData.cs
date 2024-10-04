using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;

namespace PetFamily.Application.PhotoProvider;

public record PhotoData(Stream Stream, PhotoInfo Info);

public record PhotoInfo(PhotoPath PhotoPath, string BucketName);