using Microsoft.AspNetCore.Http;

namespace PetFamily.Volunteers.Presentation.Volunteer.Requests;

public record AddPhotoToPetRequest(IFormFileCollection Photos);
    