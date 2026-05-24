using System;
using System.Collections.Generic;

namespace GymApp.Core.Entities
{
    public class WorkoutPlan
    {
        public int WpId { get; set; }
        public string ClCode { get; set; } = string.Empty;
        public string CoCode { get; set; } = string.Empty;
        public string WpName { get; set; } = string.Empty;
        public DateTime WpStartDate { get; set; }
        public DateTime WpEndDate { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual Coach? Coach { get; set; }
        public virtual ICollection<WorkoutExercise>? WorkoutExercises { get; set; }
    }
}
