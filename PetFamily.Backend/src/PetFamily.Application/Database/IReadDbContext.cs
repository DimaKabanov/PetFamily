using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dto;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    DbSet<VolunteerDto> Volunteers { get; }
}