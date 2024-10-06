using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Species;

namespace PetFamily.Infrastructure.Configurations.Write;

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

        b.ComplexProperty(s => s.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });
        
        b.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id");
    }
}