using System;

namespace GymApp.Core.Entities
{
    public class Review
    {
        public int RevId { get; set; }
        public string ClCode { get; set; } = string.Empty;
        public string CoCode { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int? SessionId { get; set; }  // Add this line
        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual Coach? Coach { get; set; }
        public virtual Session? Session { get; set; }
    }   
}
