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
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalDto dto)
        {
            try
            {
                var result = await _goalService.CreateGoalAsync(dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("client/{clientCode}")]
        public async Task<IActionResult> GetClientGoals(string clientCode)
        {
            try
            {
                var goals = await _goalService.GetClientGoalsAsync(clientCode);
                return Ok(new { success = true, count = goals.Count, data = goals });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{goalId}/progress")]
        public async Task<IActionResult> UpdateProgress(int goalId, [FromBody] UpdateGoalProgressDto dto)
        {
            try
            {
                var result = await _goalService.UpdateGoalProgressAsync(goalId, dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{goalId}")]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            try
            {
                var result = await _goalService.DeleteGoalAsync(goalId);
                return Ok(new { success = true, message = "Goal deleted successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}