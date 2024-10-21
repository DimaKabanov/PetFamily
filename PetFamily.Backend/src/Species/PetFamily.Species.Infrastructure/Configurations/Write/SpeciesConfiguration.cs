using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;

namespace PetFamily.Species.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Domain.Species>
{
    public void Configure(EntityTypeBuilder<Domain.Species> b)
    {
        b.ToTable("species");

        b.HasKey(s => s.Id);
        
        b.Property(s => s.Id)
            .HasConversion(
                speciesId => speciesId.Value,
                id => SpeciesId.Create(id)
            );

        b.ComplexProperty(s => s.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });
        
        b.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id");
    }
}