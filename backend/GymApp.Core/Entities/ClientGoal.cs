using System;

namespace GymApp.Core.Entities
{
    public class ClientGoal
    {
        public int Id { get; set; }
        public string ClientCode { get; set; } = string.Empty;
        public string GoalType { get; set; } = string.Empty;
        public decimal TargetValue { get; set; }
        public decimal? CurrentValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation property
        public virtual Client? Client { get; set; }
    }
}