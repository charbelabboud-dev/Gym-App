using System.Collections.Generic;
using System.Threading.Tasks;
using GymApp.Core.DTOs;

namespace GymApp.Core.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> AddReviewAsync(CreateReviewDto dto);
        Task<List<ReviewDto>> GetCoachReviewsAsync(string coachCode);
        Task<List<ReviewDto>> GetClientReviewsAsync(string clientCode);
        Task<CoachRatingDto> GetCoachRatingAsync(string coachCode);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}