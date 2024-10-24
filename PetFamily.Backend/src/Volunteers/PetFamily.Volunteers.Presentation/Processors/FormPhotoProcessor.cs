using Microsoft.AspNetCore.Http;
using PetFamily.Core.Dto;

namespace PetFamily.Volunteers.Presentation.Processors;

public class FormPhotoProcessor : IAsyncDisposable
{
    private readonly List<UploadPhotoDto> _photoDtos = [];

    public List<UploadPhotoDto> Process(IFormFileCollection photos)
    {
        foreach (var photo in photos)
        {
            var stream = photo.OpenReadStream();
            var photoDto = new UploadPhotoDto(stream, photo.FileName);

            _photoDtos.Add(photoDto);
        }

        return _photoDtos;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var photo in _photoDtos)
        {
            await photo.Content.DisposeAsync();
        }
    }
}