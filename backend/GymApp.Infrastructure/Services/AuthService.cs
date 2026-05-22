using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GymApp.Core.DTOs;
using GymApp.Core.Entities;
using GymApp.Core.Interfaces;
using GymApp.Infrastructure.Data;

namespace GymApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == registerDto.Email && !u.IsDeleted);
            
            if (existingUser != null)
                throw new Exception("User with this email already exists");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                UserEmail = registerDto.Email,
                UserPassword = hashedPassword,
                UserRole = registerDto.Role,
                UserStatus = true,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new AuthResponseDto
                {
                    Token = GenerateJwtToken(user),
                    UserId = user.UserId,
                    Email = user.UserEmail,
                    FullName = registerDto.FullName,
                    Role = user.UserRole
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == loginDto.Email && !u.IsDeleted);

            if (user == null)
                throw new Exception("Invalid email or password");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.UserPassword);
            
            if (!isValidPassword)
                throw new Exception("Invalid email or password");

            if (!user.UserStatus)
                throw new Exception("Account is disabled");

            return new AuthResponseDto
            {
                Token = GenerateJwtToken(user),
                UserId = user.UserId,
                Email = user.UserEmail,
                FullName = "", // Assuming FullName is not stored later on it will updated
                Role = user.UserRole
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "SuperSecretKey123!@#$%");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.UserEmail),
                    new Claim(ClaimTypes.Role, user.UserRole)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}