using System;
using System.Collections.Generic;

namespace GymApp.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public bool UserStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual Coach? Coach { get; set; }
        public virtual Dietitian? Dietitian { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}