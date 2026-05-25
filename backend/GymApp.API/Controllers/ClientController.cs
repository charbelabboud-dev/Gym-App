using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GymApp.Core.Interfaces;
using GymApp.Core.DTOs;

namespace GymApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }
[HttpGet("{clientCode}")]
public async Task<IActionResult> GetClientDetails(string clientCode)
{
    try
    {
        var client = await _clientService.GetClientDetailsAsync(clientCode);
        return Ok(new { success = true, data = client });
    }
    catch (System.Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}
[HttpGet("dietitian/{dietitianCode}")]
public async Task<IActionResult> GetClientsByDietitian(string dietitianCode)
{
    try
    {
        var clients = await _clientService.GetClientsByDietitianAsync(dietitianCode);
        return Ok(new { success = true, data = clients });
    }
    catch (System.Exception ex)
    {
        return BadRequest(new { success = false, message = ex.Message });
    }
}
        [HttpGet("coach/{coachCode}")]
        public async Task<IActionResult> GetClientsByCoach(string coachCode)
        {
            try
            {
                var clients = await _clientService.GetClientsByCoachAsync(coachCode);
                return Ok(new { success = true, data = clients });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}