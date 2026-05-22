using System;
using System.Collections.Generic;

namespace GymApp.Core.Entities
{
    public class Coach
    {
        public string CoCode { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string CoFname { get; set; } = string.Empty;
        public string CoLname { get; set; } = string.Empty;
        public DateTime CoBirthDate { get; set; }
        public string CoPhone { get; set; } = string.Empty;
        public string CoEmail { get; set; } = string.Empty;
        public string CoAddress { get; set; } = string.Empty;
        public DateTime CoHireDate { get; set; }
        public string? CoSpecialty { get; set; }
        public bool CoStatus { get; set; }
        public bool IsDeleted { get; set; }
        public decimal? Rating { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<Client>? Clients { get; set; }
        public virtual ICollection<Session>? Sessions { get; set; }
        public virtual ICollection<WorkoutPlan>? WorkoutPlans { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}