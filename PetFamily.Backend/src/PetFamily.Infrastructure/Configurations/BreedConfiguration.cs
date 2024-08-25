using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Pets;
using PetFamily.Domain.Models.Pets.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

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

        b.Property(br => br.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
    }
}