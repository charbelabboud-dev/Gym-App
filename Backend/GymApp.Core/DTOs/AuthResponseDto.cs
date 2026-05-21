namespace GymApp.Core.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? ClientCode { get; set; }
        public string? CoachCode { get; set; }
        public string? DietitianCode { get; set; }
    }
}
