using System;

namespace GymApp.Core.Entities
{
    public class Attendance
    {
        public int AttId { get; set; }
        public string ClCode { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation property
        public virtual Client? Client { get; set; }
    }
}