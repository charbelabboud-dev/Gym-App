using System;

namespace GymApp.Core.Entities
{
    public class DietPlan
    {
        public int DietId { get; set; }
        public string ClCode { get; set; } = string.Empty;
        public string DietitianId { get; set; } = string.Empty;
        public DateTime DietStartDate { get; set; }
        public DateTime DietEndDate { get; set; }
        public string DietDescription { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual Dietitian? Dietitian { get; set; }
    }
}
