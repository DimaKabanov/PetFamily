using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel.ValueObjects.EntityIds;
using PetFamily.Volunteers.Domain.Pets;
using PetFamily.Volunteers.Domain.Pets.ValueObjects;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> b)
    {
        b.ToTable("pets");

        b.HasKey(p => p.Id);

        b.Property(p => p.Id)
            .HasConversion(
                petId => petId.Value,
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

        b.Property(p => p.Photos)
            .ValueObjectsCollectionJsonConversion(
                p => new PhotoDto(p.Path.Path, p.IsMain),
                dto => new Photo(
                    PhotoPath.Create(dto.Path).Value,
                    dto.IsMain))
            .HasColumnName("photos");
        
        b.Property(p => p.Requisites)
            .ValueObjectsCollectionJsonConversion(
                r => new RequisiteDto(r.Name, r.Description),
                dto => Requisite.Create(dto.Name, dto.Description).Value)
            .HasColumnName("requisites");

        b.ComplexProperty(p => p.Properties, pb =>
        {
            pb.Property(pp => pp.SpeciesId)
                .HasConversion(
                    speciesId => speciesId.Value,
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
        
        b.ComplexProperty(p => p.Position, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("position"); 
        });
    }
}