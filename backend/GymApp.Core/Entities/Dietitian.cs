using System;
using System.Collections.Generic;

namespace GymApp.Core.Entities
{
    public class Dietitian
    {
        public string DietCode { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string DietFname { get; set; } = string.Empty;
        public string DietLname { get; set; } = string.Empty;
        public string DietEmail { get; set; } = string.Empty;
        public string DietPhone { get; set; } = string.Empty;
        public bool DietStatus { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<DietPlan>? DietPlans { get; set; }
                public virtual ICollection<Client>? Clients { get; set; }
        }
}