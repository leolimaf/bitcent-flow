using BitcentFlow.Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitcentFlow.Auth.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.FullName)
            .HasMaxLength(150);
        
        builder.Property(u => u.Birthdate)
            .HasColumnType("date");
        
        builder.Property(u => u.Token)
            .HasMaxLength(200);
        
        builder.HasIndex(u => u.Token)
            .IsUnique();
    }
}