using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        // Jadval nomi
        builder.ToTable("Subscriptions");

        // Primary Key
        builder.HasKey(s => s.Id);

        // Subscription ➝ Subscriber (User) (N:1)
        builder.HasOne(s => s.Subscriber)
            .WithMany(u => u.Subscriptions) // 👈 User entity’da ICollection<Subscription> Subscriptions bo‘lishi kerak
            .HasForeignKey(s => s.SubscriberId)
            .OnDelete(DeleteBehavior.Restrict); // ❗ Multiple cascade path'dan qochish

        // Subscription ➝ Channel (N:1)
        builder.HasOne(s => s.Channel)
            .WithMany(c => c.Subscribers) // 👈 Channel entity’da ICollection<Subscription> Subscribers bo‘lishi kerak
            .HasForeignKey(s => s.ChannelId)
            .OnDelete(DeleteBehavior.Cascade); // Channel o‘chsa, subscriptionlar ham o‘chadi
    }
}