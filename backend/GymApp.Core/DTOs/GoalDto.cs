using System;

namespace GymApp.Core.DTOs
{
    public class GoalDto
    {
        public int Id { get; set; }
        public string ClientCode { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string GoalType { get; set; } = string.Empty;
        public decimal TargetValue { get; set; }
        public decimal? CurrentValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public bool IsCompleted { get; set; }
        public int ProgressPercentage { get; set; }
    }

    public class CreateGoalDto
    {
        public string ClientCode { get; set; } = string.Empty;
        public string GoalType { get; set; } = string.Empty;
        public decimal TargetValue { get; set; }
        public DateTime? TargetDate { get; set; }
    }

    public class UpdateGoalProgressDto
    {
        public decimal CurrentValue { get; set; }
    }
}