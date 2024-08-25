using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Pets;
using PetFamily.Domain.Models.Pets.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> b)
    {
        b.ToTable("pets");

        b.HasKey(p => p.Id);

        b.Property(p => p.Id)
            .HasConversion(
                petId => petId.Id,
                id => PetId.Create(id)
            );

        b.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        b.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        
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
        
        b.OwnsOne(p => p.Details, pb =>
        {
            pb.ToJson("details");
            
            pb.OwnsMany(d => d.Photos, ppb =>
            {
                ppb.Property(pp => pp.Path)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
                
                ppb.Property(pp => pp.IsMain)
                    .IsRequired();
            });
            
            pb.OwnsMany(d => d.Requisites, rb =>
            {
                rb.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                
                rb.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
            });
        });

        b.ComplexProperty(p => p.PetProperties, pb =>
        {
            pb.Property(pp => pp.SpeciesId)
                .HasConversion(
                    speciesId => speciesId.Id,
                    id => SpeciesId.Create(id)
                )
                .IsRequired()
                .HasColumnName("species_id");
            
            pb.Property(pp => pp.BreedId)
                .HasConversion(
                    breedId => breedId.Id,
                    id => BreedId.Create(id)
                )
                .IsRequired()
                .HasColumnName("breed_id");
        });
    }
}