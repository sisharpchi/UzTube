using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Jadval nomi
        builder.ToTable("Notifications");

        // Primary key
        builder.HasKey(n => n.Id);

        // Message - majburiy
        builder.Property(n => n.Message)
            .IsRequired();

        // CreatedAt va IsRead - majburiy
        builder.Property(n => n.CreatedAt)
            .IsRequired();

        builder.Property(n => n.IsRead)
            .IsRequired();

        // Notification ➝ User (N:1)
        //builder.HasOne(n => n.User)
        //    .WithMany(u => u.Notifications) // 👈 Siz `User` entity’ga `ICollection<Notification> Notifications` qo‘shgan bo‘lishingiz kerak
        //    .HasForeignKey(n => n.UserId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
