using System;

namespace GymApp.Core.DTOs
{
    public class CoachDto
    {
        public string CoCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal Rating { get; set; } 
    }
}