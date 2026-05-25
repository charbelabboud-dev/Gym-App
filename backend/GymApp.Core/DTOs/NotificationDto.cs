using System;

namespace GymApp.Core.DTOs
{
    // What the frontend sees
    public class NotificationDto
    {
        public int NotId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;  // Session, Progress, System, etc.
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // For creating a notification
    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}