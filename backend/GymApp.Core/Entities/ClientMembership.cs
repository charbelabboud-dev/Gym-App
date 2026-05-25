using System;

namespace GymApp.Core.Entities
{
    public class ClientMembership
    {
        public int CmId { get; set; }
        public string ClCode { get; set; } = string.Empty;
        public int MemId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual MembershipPlan? MembershipPlan { get; set; }
    }
}
