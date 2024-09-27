using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Models.Volunteers;
using PetFamily.Domain.Models.Volunteers.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoService(
    IVolunteersRepository volunteersRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerService> logger)
{
    public async Task<Result<Guid, Error>> Update(
        UpdateVolunteerMainInfoRequest request,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(request.VolunteerId);
        
        var volunteerResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        var fullName = FullName.Create(
            request.Dto.FullName.Name,
            request.Dto.FullName.Surname,
            request.Dto.FullName.Patronymic).Value;
            
        var description = Description.Create(request.Dto.Description).Value;

        var experience = Experience.Create(request.Dto.Experience).Value;

        var phone = Phone.Create(request.Dto.Phone).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            description,
            experience,
            phone);

        await unitOfWork.SaveChanges(cancellationToken);

        logger.LogInformation("Updated volunteer with id: {volunteerId}", volunteerId);

        return volunteerResult.Value.Id.Value;
    }
}