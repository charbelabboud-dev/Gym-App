using System;

namespace GymApp.Core.Entities
{
    public class Payment
    {
        public int PayId { get; set; }
        public string ClCode { get; set; } = string.Empty;
        public int MemId { get; set; }
        public decimal PayAmount { get; set; }
        public string PayMethod { get; set; } = string.Empty;
        public DateTime PayDate { get; set; }
        public string PayStatus { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual Client? Client { get; set; }
        public virtual MembershipPlan? MembershipPlan { get; set; }
    }
}
