using System.Threading.Tasks;
using GymApp.Core.DTOs;

namespace GymApp.Core.Interfaces
{
    public interface IAuthService
    {
        // This interface says: Any auth service must have:
        // 1. A Register method that takes RegisterDto and returns AuthResponseDto
        // 2. A Login method that takes LoginDto and returns AuthResponseDto
        
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}