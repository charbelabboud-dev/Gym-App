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
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanService _workoutPlanService;

        public WorkoutPlanController(IWorkoutPlanService workoutPlanService)
        {
            _workoutPlanService = workoutPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPlan([FromBody] CreateWorkoutPlanDto dto)
        {
            try
            {
                var result = await _workoutPlanService.CreateWorkoutPlanAsync(dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("client/{clientCode}")]
        public async Task<IActionResult> GetClientPlans(string clientCode)
        {
            try
            {
                var plans = await _workoutPlanService.GetClientPlansAsync(clientCode);
                return Ok(new { success = true, count = plans.Count, data = plans });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("coach/{coachCode}")]
        public async Task<IActionResult> GetCoachPlans(string coachCode)
        {
            try
            {
                var plans = await _workoutPlanService.GetCoachPlansAsync(coachCode);
                return Ok(new { success = true, count = plans.Count, data = plans });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanById(int planId)
        {
            try
            {
                var plan = await _workoutPlanService.GetPlanByIdAsync(planId);
                return Ok(new { success = true, data = plan });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{planId}")]
        public async Task<IActionResult> UpdatePlan(int planId, [FromBody] UpdateWorkoutPlanDto dto)
        {
            try
            {
                var result = await _workoutPlanService.UpdatePlanAsync(planId, dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{planId}")]
        public async Task<IActionResult> DeletePlan(int planId)
        {
            try
            {
                var result = await _workoutPlanService.DeletePlanAsync(planId);
                return Ok(new { success = true, message = "Workout plan deleted successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("{planId}/exercise")]
        public async Task<IActionResult> AddExercise(int planId, [FromBody] CreateExerciseDto dto)
        {
            try
            {
                var result = await _workoutPlanService.AddExerciseToPlanAsync(planId, dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("exercise/{exerciseId}")]
        public async Task<IActionResult> RemoveExercise(int exerciseId)
        {
            try
            {
                var result = await _workoutPlanService.RemoveExerciseAsync(exerciseId);
                return Ok(new { success = true, message = "Exercise removed successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}