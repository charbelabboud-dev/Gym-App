using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GymApp.Core.DTOs;
using GymApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace GymApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookSession([FromBody] CreateSessionDto dto)
        {
            try
            {
                var result = await _sessionService.BookSessionAsync(dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("client/{clientCode}")]
        public async Task<IActionResult> GetClientSessions(string clientCode)
        {
            try
            {
                var sessions = await _sessionService.GetClientSessionsAsync(clientCode);
                return Ok(new { success = true, count = sessions.Count, data = sessions });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("coach/{coachCode}")]
        public async Task<IActionResult> GetCoachSessions(string coachCode)
        {
            try
            {
                var sessions = await _sessionService.GetCoachSessionsAsync(coachCode);
                return Ok(new { success = true, count = sessions.Count, data = sessions });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{sessionId}/reschedule")]
        public async Task<IActionResult> RescheduleSession(int sessionId, [FromBody] RescheduleSessionDto dto)
        {
            try
            {
                var result = await _sessionService.RescheduleSessionAsync(sessionId, dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

[HttpPut("{sessionId}/status")]
public async Task<IActionResult> UpdateSessionStatus(int sessionId, [FromBody] UpdateStatusDto dto)
{
    try
    {
        var result = await _sessionService.UpdateSessionStatusAsync(sessionId, dto.Status);
        return Ok(new { success = true, data = result });
    }
    catch (System.Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}


// Add this DTO class inside the same file (at the bottom)
public class UpdateStatusDto
{
    public string Status { get; set; } = string.Empty;
}
        [HttpDelete("{sessionId}/cancel")]
        public async Task<IActionResult> CancelSession(int sessionId)
        {
            try
            {
                var result = await _sessionService.CancelSessionAsync(sessionId);
                return Ok(new { success = true, message = "Session cancelled successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}