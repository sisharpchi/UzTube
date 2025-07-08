using System.Threading.Channels;

namespace Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; } // For password hashing
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public long RoleId { get; set; }
    public Role Role { get; set; }

    public long? ConfirmerId { get; set; }
    public UserConfirme? Confirmer { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; }
}


/*    public Channel Channel { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; }
    public ICollection<LikeDislike> Likes { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    public ICollection<ViewHistory> ViewHistories { get; set; } 
    public ICollection<WatchLater> WatchLaters { get; set; }
    public ICollection<SavedVideo> SavedVideos { get; set; }
    public ICollection<Notification> Notifications { get; set; }*/