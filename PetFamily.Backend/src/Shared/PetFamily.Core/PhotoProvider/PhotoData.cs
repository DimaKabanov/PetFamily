using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Core.PhotoProvider;

public record PhotoData(Stream Stream, PhotoInfo Info);

public record PhotoInfo(PhotoPath PhotoPath, string BucketName);