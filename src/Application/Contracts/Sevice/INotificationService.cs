using Application.Dtos.Notification;

namespace Application.Contracts.Sevice;

public interface INotificationService
{
    Task<bool> SendAsync(NotificationCreateDto dto);
    Task<bool> MarkAsReadAsync(long notificationId);
    Task<List<NotificationDto>> GetUserNotificationsAsync(long userId);
    Task<bool> DeleteAsync(long notificationId);
}