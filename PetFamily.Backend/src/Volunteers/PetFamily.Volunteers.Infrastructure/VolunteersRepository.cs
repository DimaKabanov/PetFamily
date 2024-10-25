using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.Volunteers.Infrastructure;

public class VolunteersRepository(WriteDbContext dbContext) : IVolunteersRepository
{
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken ct)
    {
        await dbContext.Volunteers.AddAsync(volunteer, ct);

        return volunteer.Id.Value;
    }
    
    public Guid Save(Volunteer volunteer, CancellationToken ct)
    {
        dbContext.Volunteers.Attach(volunteer);

        return volunteer.Id.Value;
    }

    public Guid Delete(Volunteer volunteer, CancellationToken ct)
    {
        dbContext.Volunteers.Remove(volunteer);

        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken ct)
    {
        var volunteer = await dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, ct);

        if (volunteer is null)
            return Errors.General.NotFound(volunteerId.Value);

        return volunteer;
    }
}