namespace PetFamily.Application.Files.Upload;

public record UploadFileRequest(
    Stream stream,
    string bucketName,
    string fileName);