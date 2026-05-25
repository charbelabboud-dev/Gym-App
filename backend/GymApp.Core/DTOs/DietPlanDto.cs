using System;

namespace GymApp.Core.DTOs
{
    // What the frontend sees
    public class DietPlanDto
    {
        public int DietId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string DietitianName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public int? CaloriesTarget { get; set; }
    }

    // What dietitian sends when creating a plan
    public class CreateDietPlanDto
    {
        public string ClientCode { get; set; } = string.Empty;
        public string DietitianCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? CaloriesTarget { get; set; }
    }

    // For updating a plan
    public class UpdateDietPlanDto
    {
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? CaloriesTarget { get; set; }
    }
}