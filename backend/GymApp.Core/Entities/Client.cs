using System;
using System.Collections.Generic;

namespace GymApp.Core.Entities
{
    public class Client
    {
        public string ClCode { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string ClFname { get; set; } = string.Empty;
        public string ClLname { get; set; } = string.Empty;
        public DateTime ClBirthDate { get; set; }
        public string ClPhone { get; set; } = string.Empty;
        public string ClAddress { get; set; } = string.Empty;
        public DateTime ClRegisterDate { get; set; }
        public bool ClStatus { get; set; }
        public string? ClCoachId { get; set; }
        public int? ClMembershipId { get; set; }
        public string? DietitianId { get; set; } 
        public virtual Dietitian? Dietitian { get; set; } 
        public bool IsDeleted { get; set; }
        // Navigation properties
        public virtual ICollection<ProgressEntry>? ProgressEntries { get; set; }
        public virtual User? User { get; set; }
        public virtual Coach? Coach { get; set; }
        public virtual MembershipPlan? Membership { get; set; }
        public virtual ICollection<ClientMembership>? ClientMemberships { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<Session>? Sessions { get; set; }
        public virtual ICollection<Attendance>? Attendances { get; set; }
        public virtual ICollection<WorkoutPlan>? WorkoutPlans { get; set; }
        public virtual ICollection<DietPlan>? DietPlans { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}