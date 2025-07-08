using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ConfirmerConfiguration : IEntityTypeConfiguration<UserConfirme>
{
    public void Configure(EntityTypeBuilder<UserConfirme> builder)
    {
        builder.ToTable("Confirmers");
        builder.HasKey(uc => uc.ConfirmerId);

        builder.Property(uc => uc.ConfirmingCode)
            .IsRequired(false)
            .HasMaxLength(6);

        builder.HasIndex(uc => uc.Gmail)
            .IsUnique()
            .HasFilter("\"IsConfirmed\" = true");

        builder.Property(uc => uc.ExpiredDate)
            .IsRequired()
            .HasDefaultValueSql("NOW() + INTERVAL '10 minutes'");


        builder.Property(isc => isc.IsConfirmed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(uc => uc.User)
            .WithOne(u => u.Confirmer)
            .HasForeignKey<UserConfirme>(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
