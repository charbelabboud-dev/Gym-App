using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GymApp.Core.DTOs;
using GymApp.Core.Interfaces;

namespace GymApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDto dto)
        {
            try
            {
                var result = await _reviewService.AddReviewAsync(dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("coach/{coachCode}")]
        public async Task<IActionResult> GetCoachReviews(string coachCode)
        {
            try
            {
                var reviews = await _reviewService.GetCoachReviewsAsync(coachCode);
                return Ok(new { success = true, count = reviews.Count, data = reviews });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("client/{clientCode}")]
        public async Task<IActionResult> GetClientReviews(string clientCode)
        {
            try
            {
                var reviews = await _reviewService.GetClientReviewsAsync(clientCode);
                return Ok(new { success = true, count = reviews.Count, data = reviews });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("coach/{coachCode}/rating")]
        public async Task<IActionResult> GetCoachRating(string coachCode)
        {
            try
            {
                var rating = await _reviewService.GetCoachRatingAsync(coachCode);
                return Ok(new { success = true, data = rating });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            try
            {
                var result = await _reviewService.DeleteReviewAsync(reviewId);
                return Ok(new { success = true, message = "Review deleted successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}