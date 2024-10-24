using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dto;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> b)
    {
        b.ToTable("pets");

        b.HasKey(p => p.Id);
        
        b.Property(p => p.Photos)
            .HasConversion(
                ph => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<PhotoDto[]>(json, JsonSerializerOptions.Default)!);
        
        b.Property(p => p.Requisites)
            .HasConversion(
                r => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);
    }
}