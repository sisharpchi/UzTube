namespace Application.Dtos.Notification;

public class NotificationDto
{
    public long Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }

    public long UserId { get; set; }
    public string Username { get; set; } // optional: foydalanuvchi ismi ko‘rsatish uchun
}