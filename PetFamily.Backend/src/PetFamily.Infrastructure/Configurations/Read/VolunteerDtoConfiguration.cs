using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dto;

namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> b)
    {
        b.ToTable("volunteers");

        b.HasKey(v => v.Id);
        
        b.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey(p => p.VolunteerId);
        
        b.Property(v => v.SocialNetworks)
            .HasConversion(
                s => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<SocialNetworkDto[]>(json, JsonSerializerOptions.Default)!);
        
        b.Property(v => v.Requisites)
            .HasConversion(
                r => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);
    }
}