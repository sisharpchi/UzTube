﻿// <auto-generated />
using System;
using Infrastructure.Persistence.MainDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistence.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250704104341_InitUpdateAppDb")]
    partial class InitUpdateAppDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Channel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Channels", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ParentCommentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("VideoId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CommentLike", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CommentId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentLikes", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.LikeDislike", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsLike")
                        .HasColumnType("bit");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("LikeDislikes", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Playlist", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ChannelId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Playlists", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.SavedVideo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("SavedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("SavedVideos", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Subscription", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ChannelId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SubscribedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("SubscriberId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Subscriptions", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Video", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ChannelId")
                        .HasColumnType("bigint");

                    b.Property<string>("CloudPublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<long?>("PlaylistId")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("Videos", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.VideoReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ReporterId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("VideoReports", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.VideoTag", b =>
                {
                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.Property<long>("TagId")
                        .HasColumnType("bigint");

                    b.HasKey("VideoId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("VideoTags", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.ViewHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("SecondsWatched")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("WatchedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("ViewHistories", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.WatchLater", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("VideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("WatchLaters", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.HasOne("Domain.Entities.Comment", "ParentComment")
                        .WithMany("Replies")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Domain.Entities.Video", "Video")
                        .WithMany("Comments")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Domain.Entities.CommentLike", b =>
                {
                    b.HasOne("Domain.Entities.Comment", "Comment")
                        .WithMany("Likes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("Domain.Entities.LikeDislike", b =>
                {
                    b.HasOne("Domain.Entities.Video", "Video")
                        .WithMany("Likes")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Domain.Entities.Playlist", b =>
                {
                    b.HasOne("Domain.Entities.Channel", "Channel")
                        .WithMany("Playlists")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("Domain.Entities.SavedVideo", b =>
                {
                    b.HasOne("Domain.Entities.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Domain.Entities.Subscription", b =>
                {
                    b.HasOne("Domain.Entities.Channel", "Channel")
                        .WithMany("Subscribers")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("Domain.Entities.Video", b =>
                {
                    b.HasOne("Domain.Entities.Channel", "Channel")
                        .WithMany("Videos")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Playlist", "Playlist")
                        .WithMany("Videos")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Channel");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("Domain.Entities.VideoReport", b =>
                {
                    b.HasOne("Domain.Entities.Video", "Video")
                        .WithMany("Reports")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Domain.Entities.VideoTag", b =>
                {
                    b.HasOne("Domain.Entities.Tag", "Tag")
                        .WithMany("VideoTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Video", "Video")
                        .WithMany("VideoTags")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Domain.Entities.ViewHistory", b =>
                {
                    b.HasOne("Domain.Entities.Video", "Video")
                        .WithMany("ViewHistories")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Domain.Entities.WatchLater", b =>
                {
                    b.HasOne("Domain.Entities.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Domain.Entities.Channel", b =>
                {
                    b.Navigation("Playlists");

                    b.Navigation("Subscribers");

                    b.Navigation("Videos");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Domain.Entities.Playlist", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("Domain.Entities.Tag", b =>
                {
                    b.Navigation("VideoTags");
                });

            modelBuilder.Entity("Domain.Entities.Video", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Reports");

                    b.Navigation("VideoTags");

                    b.Navigation("ViewHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
