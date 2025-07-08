namespace Domain.Entities;

public class Notification
{
    public long Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }

    public long UserId { get; set; }
}
