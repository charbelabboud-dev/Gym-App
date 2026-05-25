using System;

namespace GymApp.Core.Entities
{
    public class Notification
    {
        public int NotId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;  // Add this
        public string Message { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;  // Add this
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation property
        public virtual User? User { get; set; }
    }
}