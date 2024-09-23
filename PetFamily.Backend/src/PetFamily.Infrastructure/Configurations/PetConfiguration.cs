using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Species;
using PetFamily.Domain.Models.Volunteers.Pets;
using PetFamily.Domain.Models.Volunteers.Pets.ValueObjects;
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

        b.ComplexProperty(p => p.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });

        b.ComplexProperty(p => p.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description"); 
        });

        b.ComplexProperty(p => p.PhysicalProperty, ppb =>
        {
            ppb.Property(pp => pp.Color)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("color");
        
            ppb.Property(pp => pp.Health)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("health");
        
            ppb.Property(pp => pp.Weight)
                .IsRequired()
                .HasColumnName("weight");
        
            ppb.Property(pp => pp.Height)
                .IsRequired()
                .HasColumnName("height");
        });

        b.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH)
                .HasColumnName("street");
            
            ab.Property(a => a.Home)
                .IsRequired()
                .HasColumnName("home");
            
            ab.Property(a => a.Flat)
                .IsRequired()
                .HasColumnName("flat");
        });

        b.ComplexProperty(p => p.Phone, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("phone");
        });
        
        b.Property(p => p.IsCastrated)
            .IsRequired();
        
        b.ComplexProperty(p => p.DateOfBirth, pb =>
        {
            pb.Property(d => d.Value)
                .IsRequired()
                .HasColumnName("date_of_birth");
        });
        
        b.Property(p => p.IsVaccinated)
            .IsRequired();
        
        b.Property(p => p.AssistanceStatus)
            .IsRequired();
        
        b.ComplexProperty(p => p.CreatedDate, pb =>
        {
            pb.Property(d => d.Value)
                .IsRequired()
                .HasColumnName("created_date");
        });
        
        b.OwnsOne(p => p.PhotoList, pb =>
        {
            pb.ToJson("photo_list");
            
            pb.OwnsMany(d => d.Photos, ppb =>
            {
                ppb.Property(pp => pp.Path)
                    .HasConversion(
                        p => p.Path,
                        value => FilePath.Create(value).Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
                
                ppb.Property(pp => pp.IsMain)
                    .IsRequired();
            });
        });
        
        b.OwnsOne(p => p.RequisiteList, pb =>
        {
            pb.ToJson("requisite_list");

            pb.OwnsMany(r => r.Requisites, rb =>
            {
                rb.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                
                rb.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
            });
        });

        b.ComplexProperty(p => p.Properties, pb =>
        {
            pb.Property(pp => pp.SpeciesId)
                .HasConversion(
                    speciesId => speciesId.Id,
                    id => SpeciesId.Create(id)
                )
                .IsRequired()
                .HasColumnName("species_id");
            
            pb.Property(pp => pp.BreedId)
                .IsRequired()
                .HasColumnName("breed_id");
        });
        
        b.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}