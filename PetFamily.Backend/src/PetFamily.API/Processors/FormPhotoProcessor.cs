using PetFamily.Application.Volunteers.AddPet;

namespace PetFamily.API.Processors;

public class FormPhotoProcessor : IAsyncDisposable
{
    private readonly List<CreatePhotoDto> _photoDtos = [];

    public List<CreatePhotoDto> Process(IFormFileCollection photos)
    {
        foreach (var photo in photos)
        {
            var stream = photo.OpenReadStream();
            var photoDto = new CreatePhotoDto(stream, photo.FileName);

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