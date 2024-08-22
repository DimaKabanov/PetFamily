using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models;

namespace PetFamily.Infrastructure;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Pet> Pets => Set<Pet>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => { builder.AddConsole(); });
}