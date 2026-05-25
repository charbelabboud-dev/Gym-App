using System.Collections.Generic;

namespace GymApp.Core.Entities
{
    public class MembershipPlan
    {
        public int MemId { get; set; }
        public string MemName { get; set; } = string.Empty;
        public decimal MemPrice { get; set; }
        public int MemDurationDays { get; set; }
        public string? MemDescription { get; set; }
        public bool MemStatus { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual ICollection<Client>? Clients { get; set; }
        public virtual ICollection<ClientMembership>? ClientMemberships { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}