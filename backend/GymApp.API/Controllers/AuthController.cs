using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GymApp.Core.DTOs;
using GymApp.Core.Interfaces;

namespace GymApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // We need a service to handle authentication
        private readonly IAuthService _authService;

        // Constructor - receives the service when the controller is created
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Call the service to register the user
                var result = await _authService.RegisterAsync(registerDto);
                
                // Return success response with user data
                return Ok(new { 
                    success = true, 
                    message = "Registration successful",
                    data = result 
                });
            }
            catch (System.Exception ex)
            {
                // Return error response
                return BadRequest(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Call the service to login
                var result = await _authService.LoginAsync(loginDto);
                
                // Return success response with token
                return Ok(new { 
                    success = true, 
                    message = "Login successful",
                    data = result 
                });
            }
            catch (System.Exception ex)
            {
                // Return error response
                return BadRequest(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }
    }
}