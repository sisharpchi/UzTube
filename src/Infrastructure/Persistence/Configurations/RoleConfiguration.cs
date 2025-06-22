using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Jadval nomi
        builder.ToTable("Roles");

        // Primary key
        builder.HasKey(r => r.Id);

        // Name - majburiy
        builder.Property(r => r.Name)
            .IsRequired();

        // Description - optional
        builder.Property(r => r.Description)
            .IsRequired(false);

        // Userlar bilan bog‘lanish: Role ➝ Users (1:N)
        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Cascade); // Har bir user o‘z role'iga bog‘langan
    }
}