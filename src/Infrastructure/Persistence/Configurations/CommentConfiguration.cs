using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Jadval nomi
        builder.ToTable("Comments");

        // Primary key
        builder.HasKey(c => c.Id);

        // Text
        builder.Property(c => c.Text)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        //// Comment ➝ Video (N:1)
        //builder.HasOne(c => c.Video)
        //    .WithMany(v => v.Comments)
        //    .HasForeignKey(c => c.VideoId)
        //    .OnDelete(DeleteBehavior.NoAction); //Restrict ❗ Prevent multiple cascade path

        // Comment ➝ User (N:1)
        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); // ❗ Prevent multiple cascade path

        // Comment ➝ ParentComment (recursive self-reference)
        builder.HasOne(c => c.ParentComment)
            .WithMany(p => p.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.NoAction); // ❗ Recursive delete can be dangerous

        //// Comment ➝ CommentLikes (1:N)
        //builder.HasMany(c => c.Likes)
        //    .WithOne(cl => cl.Comment)
        //    .HasForeignKey(cl => cl.CommentId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}