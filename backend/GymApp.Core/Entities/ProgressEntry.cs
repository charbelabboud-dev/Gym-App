using System;

namespace GymApp.Core.Entities
{
    public class ProgressEntry
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? Chest { get; set; }
        public decimal? Waist { get; set; }
        public string? Notes { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Client? Client { get; set; }
    }
}