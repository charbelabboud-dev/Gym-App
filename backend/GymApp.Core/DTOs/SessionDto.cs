using System;

namespace GymApp.Core.DTOs
{
    public class SessionDto
    {
        public int SesId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string CoachName { get; set; } = string.Empty;
        public string SessionType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
    }
        public class CreateSessionDto
    {
        public string ClientCode { get; set; } = string.Empty;
        public string CoachCode { get; set; } = string.Empty;
        public string SessionType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
    }

    public class RescheduleSessionDto 
    {
        public DateTime Date {get; set;} 
        public int Duration{get;set;}
    }
}