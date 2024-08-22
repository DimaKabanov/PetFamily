using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> b)
    {
        b.ToTable("pets");

        b.HasKey(p => p.Id);

        b.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        b.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        b.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        
        b.Property(p => p.Breed)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        b.Property(p => p.Color)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        b.Property(p => p.Health)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        b.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
        
        b.Property(p => p.Weight)
            .IsRequired();
        
        b.Property(p => p.Height)
            .IsRequired();
        
        b.Property(p => p.Phone)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        b.Property(p => p.IsCastrated)
            .IsRequired();
        
        b.Property(p => p.DateOfBirth)
            .IsRequired();
        
        b.Property(p => p.IsVaccinated)
            .IsRequired();
        
        b.Property(p => p.AssistanceStatus)
            .IsRequired();
        
        b.Property(p => p.CreatedDate)
            .IsRequired();

        b.HasMany(p => p.Requisites)
            .WithOne()
            .HasForeignKey("pet_id");
        
        b.HasMany(p => p.Photos)
            .WithOne()
            .HasForeignKey("pet_id");
    }
}