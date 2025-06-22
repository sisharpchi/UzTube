using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
    public void Configure(EntityTypeBuilder<CommentLike> builder)
    {
        // Jadval nomi
        builder.ToTable("CommentLikes");

        // Primary Key
        builder.HasKey(cl => cl.Id);

        // CommentLike ➝ Comment (N:1)
        builder.HasOne(cl => cl.Comment)
            .WithMany(c => c.Likes)
            .HasForeignKey(cl => cl.CommentId)
            .OnDelete(DeleteBehavior.Cascade); // Comment o‘chsa, like-lar ham o‘chadi

        // CommentLike ➝ User (N:1)
        builder.HasOne(cl => cl.User)
            .WithMany() // Userda alohida navigation yo‘q bo‘lsa
            .HasForeignKey(cl => cl.UserId)
            .OnDelete(DeleteBehavior.Restrict); // User o‘chsa, like-lar ham o‘chadi
    }
}
