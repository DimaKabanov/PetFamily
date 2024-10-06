using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Volunteers;

namespace PetFamily.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> b)
    {
        b.ToTable("volunteers");

        b.HasKey(v => v.Id);
        
        b.Property(v => v.Id)
            .HasConversion(
                volunteerId => volunteerId.Value,
                id => VolunteerId.Create(id)
            );

        b.ComplexProperty(v => v.FullName, fnb =>
        {
            fnb.Property(fn => fn.Name)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
            
            fnb.Property(fn => fn.Surname)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("surname");
            
            fnb.Property(fn => fn.Patronymic)
                .IsRequired(false)
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("patronymic");
        });

        b.ComplexProperty(v => v.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description");
        });

        b.ComplexProperty(v => v.Experience, eb =>
        {
            eb.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("experience");
        });

        b.ComplexProperty(v => v.Phone, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("phone");
        });
        
        b.OwnsOne(v => v.SocialNetworks, vb =>
        {
            vb.ToJson("social_networks");
            
            vb.OwnsMany(d => d.Values, sb =>
            {
                sb.Property(s => s.Title)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
                
                sb.Property(s => s.Url)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
            });
        });
        
        b.OwnsOne(v => v.Requisites, vb =>
        {
            vb.ToJson("requisites");

            vb.OwnsMany(d => d.Values, rb =>
            {
                rb.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
                
                rb.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MAX_HIGH_TEXT_LENGTH);
            });
        });
        
        b.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        b.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}