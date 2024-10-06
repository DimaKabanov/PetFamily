using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Species.Breeds;

namespace PetFamily.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> b)
    {
        b.ToTable("breed");

        b.HasKey(br => br.Id);
        
        b.Property(br => br.Id)
            .HasConversion(
                breedId => breedId.Id,
                id => BreedId.Create(id)
            );

        b.ComplexProperty(br => br.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });
    }
}