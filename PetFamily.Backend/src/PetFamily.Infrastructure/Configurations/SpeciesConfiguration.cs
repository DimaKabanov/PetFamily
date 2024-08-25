using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Pets;
using PetFamily.Domain.Models.Pets.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> b)
    {
        b.ToTable("species");

        b.HasKey(s => s.Id);
        
        b.Property(s => s.Id)
            .HasConversion(
                speciesId => speciesId.Id,
                id => SpeciesId.Create(id)
            );

        b.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        b.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id");
    }
}