using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.FullName)
            .IsRequired().HasMaxLength(90);

        builder.Property(u => u.Email)
            .IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.ProfileImageUrl)
            .IsRequired(false);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        // Relationships

        // Role: User ➝ Role (many-to-one)
        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        //// Channel: User ➝ Channel (one-to-one)
        //builder.HasOne(u => u.Channel)
        //    .WithOne(c => c.Owner)
        //    .HasForeignKey<Channel>(c => c.OwnerId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //// Comments: User ➝ Comments (one-to-many)
        //builder.HasMany(u => u.Comments)
        //    .WithOne(c => c.User)
        //    .HasForeignKey(c => c.UserId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //// LikeDislikes: User ➝ LikeDislikes (one-to-many)
        //builder.HasMany(u => u.Likes)
        //    .WithOne(ld => ld.User)
        //    .HasForeignKey(ld => ld.UserId)
        //    .OnDelete(DeleteBehavior.Restrict); // ❗ Prevent multiple cascade paths

        //// Subscriptions: User ➝ Subscriptions (one-to-many)
        //builder.HasMany(u => u.Subscriptions)
        //    .WithOne(s => s.Subscriber)
        //    .HasForeignKey(s => s.SubscriberId)
        //    .OnDelete(DeleteBehavior.Restrict); // ❗ Prevent multiple cascade paths

        //// ViewHistories: User ➝ ViewHistories
        //builder.HasMany(u => u.ViewHistories)
        //    .WithOne(vh => vh.User)
        //    .HasForeignKey(vh => vh.UserId)
        //    .OnDelete(DeleteBehavior.Restrict); // yoki Cascade, agar kerak bo‘lsa

        //// WatchLaters
        //builder.HasMany(u => u.WatchLaters)
        //    .WithOne(wl => wl.User)
        //    .HasForeignKey(wl => wl.UserId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //// SavedVideos
        //builder.HasMany(u => u.SavedVideos)
        //    .WithOne(sv => sv.User)
        //    .HasForeignKey(sv => sv.UserId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder.HasMany(u => u.Notifications)
        //    .WithOne(n => n.User)
        //    .HasForeignKey(n => n.UserId)
        //    .OnDelete(DeleteBehavior.Restrict); // ❗ Prevent multiple cascade paths

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Restrict); // ❗ Prevent multiple cascade paths


        builder.HasOne(u => u.Confirmer)
            .WithOne(c => c.User)
            .HasForeignKey<User>(u => u.ConfirmerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
