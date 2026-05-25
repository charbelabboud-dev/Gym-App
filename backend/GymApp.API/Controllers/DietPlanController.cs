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
    public class DietPlanController : ControllerBase
    {
        private readonly IDietPlanService _dietPlanService;

        public DietPlanController(IDietPlanService dietPlanService)
        {
            _dietPlanService = dietPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDietPlan([FromBody] CreateDietPlanDto dto)
        {
            try
            {
                var result = await _dietPlanService.CreateDietPlanAsync(dto);
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
                var plans = await _dietPlanService.GetClientPlansAsync(clientCode);
                return Ok(new { success = true, count = plans.Count, data = plans });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("dietitian/{dietitianCode}")]
        public async Task<IActionResult> GetDietitianPlans(string dietitianCode)
        {
            try
            {
                var plans = await _dietPlanService.GetDietitianPlansAsync(dietitianCode);
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
                var plan = await _dietPlanService.GetPlanByIdAsync(planId);
                return Ok(new { success = true, data = plan });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{planId}")]
        public async Task<IActionResult> UpdatePlan(int planId, [FromBody] UpdateDietPlanDto dto)
        {
            try
            {
                var result = await _dietPlanService.UpdatePlanAsync(planId, dto);
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
                var result = await _dietPlanService.DeletePlanAsync(planId);
                return Ok(new { success = true, message = "Diet plan deleted successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}