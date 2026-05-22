using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GymApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GymApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoachController : ControllerBase
    {
        private readonly ICoachService _coachService;

        public CoachController(ICoachService coachService)
        {
            _coachService = coachService;
        }

[HttpGet("top-rated")]
public async Task<IActionResult> GetTopRatedCoaches([FromQuery] int limit = 10)
{
    try
    {
        var coaches = await _coachService.GetTopRatedCoachesAsync(limit);
        return Ok(new { success = true, data = coaches });
    }
    catch (System.Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}
        [HttpGet]
        public async Task<IActionResult> GetAllCoaches()
        {
            try
            {
                var coaches = await _coachService.GetAllCoachesAsync();
                return Ok(new { success = true, data = coaches });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // GET: api/coach/specialty/{specialty}
[HttpGet("specialty/{specialty}")]
public async Task<IActionResult> GetCoachesBySpecialty(string specialty)
{
    try
    {
        var coaches = await _coachService.GetCoachesBySpecialtyAsync(specialty);
        
        if (coaches.Count == 0)
        {
            return Ok(new { 
                success = true, 
                message = $"No coaches found with specialty: {specialty}",
                data = coaches 
            });
        }
        
        return Ok(new { success = true, count = coaches.Count, data = coaches });
    }
    catch (System.Exception ex)
    {
        return StatusCode(500, new { success = false, message = ex.Message });
    }
}
    }
}