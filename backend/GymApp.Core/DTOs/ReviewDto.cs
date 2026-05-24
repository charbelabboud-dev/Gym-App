using System;

namespace GymApp.Core.DTOs
{
    // What the frontend sees
    public class ReviewDto
    {
        public int RevId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string CoachName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? SessionId { get; set; }
    }

    // What client sends when creating a review
    public class CreateReviewDto
    {
        public string ClientCode { get; set; } = string.Empty;
        public string CoachCode { get; set; } = string.Empty;
        public int Rating { get; set; }  // 1 to 5
        public string? Comment { get; set; }
        public int? SessionId { get; set; }  // Optional - link to completed session
    }

    // Coach rating summary
    public class CoachRatingDto
    {
        public string CoachCode { get; set; } = string.Empty;
        public string CoachName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int Rating1Count { get; set; }
        public int Rating2Count { get; set; }
        public int Rating3Count { get; set; }
        public int Rating4Count { get; set; }
        public int Rating5Count { get; set; }
    }
}