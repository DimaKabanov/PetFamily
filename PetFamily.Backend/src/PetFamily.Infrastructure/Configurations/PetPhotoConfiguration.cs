using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
{
    public void Configure(EntityTypeBuilder<PetPhoto> b)
    {
        b.ToTable("PetPhotos");

        b.HasKey(p => p.Id);

        b.Property(p => p.Path)
            .IsRequired()
            .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
        
        b.Property(p => p.IsMain)
            .IsRequired();
    }
}