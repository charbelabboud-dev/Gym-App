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
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProgress([FromBody] CreateProgressDto dto)
        {
            try
            {
                var result = await _progressService.AddProgressAsync(dto);
                return Ok(new { success = true, data = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("client/{clientCode}")]
        public async Task<IActionResult> GetClientProgress(string clientCode)
        {
            try
            {
                var progress = await _progressService.GetClientProgressAsync(clientCode);
                return Ok(new { success = true, count = progress.Count, data = progress });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgressById(int id)
        {
            try
            {
                var progress = await _progressService.GetProgressByIdAsync(id);
                return Ok(new { success = true, data = progress });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgress(int id)
        {
            try
            {
                var result = await _progressService.DeleteProgressAsync(id);
                return Ok(new { success = true, message = "Progress entry deleted successfully" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}