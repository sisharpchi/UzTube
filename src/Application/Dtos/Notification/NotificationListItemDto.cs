namespace Application.Dtos.Notification;

public class NotificationListItemDto
{
    public long Id { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
