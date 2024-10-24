using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Species.Domain.Breeds;

namespace PetFamily.Species.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> b)
    {
        b.ToTable("breeds");

        b.HasKey(br => br.Id);
        
        b.Property(br => br.Id)
            .HasConversion(
                breedId => breedId.Value,
                id => BreedId.Create(id)
            );

        b.ComplexProperty(br => br.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });
    }
}