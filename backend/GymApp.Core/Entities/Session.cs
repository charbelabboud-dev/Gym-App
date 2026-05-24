using System;

namespace GymApp.Core.Entities
{
    public class Session
    {
        public int SesId { get; set; }
        public string SesClCode { get; set; } = string.Empty;
        public string SesCoCode { get; set; } = string.Empty;
        public string SesType { get; set; } = string.Empty;
        public string? SesDescription { get; set; }
        public DateTime SesDateTime { get; set; }
        public int SesDuration { get; set; }
        public string SesStatus { get; set; } = "Scheduled";
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual Coach? Coach { get; set; }
    }
}
