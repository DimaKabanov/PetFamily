using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class RequisiteConfiguration : IEntityTypeConfiguration<Requisite>
{
    public void Configure(EntityTypeBuilder<Requisite> b)
    {
        b.ToTable("requisites");

        b.HasKey(r => r.Id);

        b.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        b.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
    }
}