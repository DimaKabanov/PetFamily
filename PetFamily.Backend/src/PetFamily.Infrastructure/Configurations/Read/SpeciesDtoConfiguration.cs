using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dto;

namespace PetFamily.Infrastructure.Configurations.Read;

public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> b)
    {
        b.ToTable("species");

        b.HasKey(s => s.Id);
        
        b.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey(breed => breed.SpeciesId);
    }
}