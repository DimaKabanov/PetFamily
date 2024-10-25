using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.SharedKernel.PhotoProvider;

public record PhotoData(Stream Stream, PhotoInfo Info);

public record PhotoInfo(PhotoPath PhotoPath, string BucketName);