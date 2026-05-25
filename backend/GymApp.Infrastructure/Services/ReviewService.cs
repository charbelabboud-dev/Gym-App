using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GymApp.Core.DTOs;
using GymApp.Core.Entities;
using GymApp.Core.Interfaces;
using GymApp.Infrastructure.Data;

namespace GymApp.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewDto> AddReviewAsync(CreateReviewDto dto)
        {
            // Check if client exists
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ClCode == dto.ClientCode && !c.IsDeleted);
            
            if (client == null)
                throw new Exception($"Client with code {dto.ClientCode} not found");

            // Check if coach exists
            var coach = await _context.Coaches
                .FirstOrDefaultAsync(c => c.CoCode == dto.CoachCode && !c.IsDeleted);
            
            if (coach == null)
                throw new Exception($"Coach with code {dto.CoachCode} not found");

            // Validate rating (1-5)
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new Exception("Rating must be between 1 and 5");

            // Create review
            var review = new Review
            {
                ClCode = dto.ClientCode,
                CoCode = dto.CoachCode,
                Rating = dto.Rating,
                Comment = dto.Comment,
                SessionId = dto.SessionId,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Update coach's average rating
            await UpdateCoachAverageRating(dto.CoachCode);

            return new ReviewDto
            {
                RevId = review.RevId,
                ClientName = $"{client.ClFname} {client.ClLname}",
                CoachName = $"{coach.CoFname} {coach.CoLname}",
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedDate = review.CreatedDate,
                SessionId = review.SessionId
            };
        }

        public async Task<List<ReviewDto>> GetCoachReviewsAsync(string coachCode)
        {
            var reviews = await _context.Reviews
                .Where(r => r.CoCode == coachCode && !r.IsDeleted)
                .Include(r => r.Client)
                    .ThenInclude(c => c.User)
                .Include(r => r.Coach)
                    .ThenInclude(c => c.User)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return reviews.Select(r => new ReviewDto
            {
                RevId = r.RevId,
                ClientName = $"{r.Client.ClFname} {r.Client.ClLname}",
                CoachName = $"{r.Coach.CoFname} {r.Coach.CoLname}",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedDate = r.CreatedDate,
                SessionId = r.SessionId
            }).ToList();
        }

        public async Task<List<ReviewDto>> GetClientReviewsAsync(string clientCode)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ClCode == clientCode && !r.IsDeleted)
                .Include(r => r.Client)
                    .ThenInclude(c => c.User)
                .Include(r => r.Coach)
                    .ThenInclude(c => c.User)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return reviews.Select(r => new ReviewDto
            {
                RevId = r.RevId,
                ClientName = $"{r.Client.ClFname} {r.Client.ClLname}",
                CoachName = $"{r.Coach.CoFname} {r.Coach.CoLname}",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedDate = r.CreatedDate,
                SessionId = r.SessionId
            }).ToList();
        }

        public async Task<CoachRatingDto> GetCoachRatingAsync(string coachCode)
        {
            var coach = await _context.Coaches
                .FirstOrDefaultAsync(c => c.CoCode == coachCode && !c.IsDeleted);
            
            if (coach == null)
                throw new Exception($"Coach with code {coachCode} not found");

            var reviews = await _context.Reviews
                .Where(r => r.CoCode == coachCode && !r.IsDeleted)
                .ToListAsync();

            var coachWithUser = await _context.Coaches
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CoCode == coachCode);

            if (!reviews.Any())
            {
                return new CoachRatingDto
                {
                    CoachCode = coachCode,
                    CoachName = $"{coachWithUser.CoFname} {coachWithUser.CoLname}",
                    AverageRating = 0,
                    TotalReviews = 0,
                    Rating1Count = 0,
                    Rating2Count = 0,
                    Rating3Count = 0,
                    Rating4Count = 0,
                    Rating5Count = 0
                };
            }

            return new CoachRatingDto
            {
                CoachCode = coachCode,
                CoachName = $"{coachWithUser.CoFname} {coachWithUser.CoLname}",
                AverageRating = reviews.Average(r => r.Rating),
                TotalReviews = reviews.Count,
                Rating1Count = reviews.Count(r => r.Rating == 1),
                Rating2Count = reviews.Count(r => r.Rating == 2),
                Rating3Count = reviews.Count(r => r.Rating == 3),
                Rating4Count = reviews.Count(r => r.Rating == 4),
                Rating5Count = reviews.Count(r => r.Rating == 5)
            };
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.RevId == reviewId && !r.IsDeleted);

            if (review == null)
                throw new Exception($"Review with ID {reviewId} not found");

            review.IsDeleted = true;
            await _context.SaveChangesAsync();

            // Update coach's average rating after deletion
            await UpdateCoachAverageRating(review.CoCode);

            return true;
        }

        private async Task UpdateCoachAverageRating(string coachCode)
        {
            var reviews = await _context.Reviews
                .Where(r => r.CoCode == coachCode && !r.IsDeleted)
                .ToListAsync();

            var coach = await _context.Coaches
                .FirstOrDefaultAsync(c => c.CoCode == coachCode);

            if (coach != null)
            {
                coach.Rating = reviews.Any() ? (decimal?)reviews.Average(r => r.Rating) : null;
                await _context.SaveChangesAsync();
            }
        }
    }
}