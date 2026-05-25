using System;

namespace GymApp.Core.DTOs
{
    public class ClientDto
    {
        public string ClCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime RegisterDate { get; set; }
        public int TotalSessions { get; set; }
        public int CompletedSessions { get; set; }
    }
}