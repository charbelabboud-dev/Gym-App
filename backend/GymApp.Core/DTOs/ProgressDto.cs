using System;

namespace GymApp.Core.DTOs
{
    // What the frontend sees
    public class ProgressDto
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public decimal? Weight { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? Chest { get; set; }
        public decimal? Waist { get; set; }
        public string? Notes { get; set; }
        public DateTime EntryDate { get; set; }
    }

    // What client sends when adding progress
    public class CreateProgressDto
    {
        public string ClientCode { get; set; } = string.Empty;
        public decimal? Weight { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? Chest { get; set; }
        public decimal? Waist { get; set; }
        public string? Notes { get; set; }
        public DateTime EntryDate { get; set; }
    }
}