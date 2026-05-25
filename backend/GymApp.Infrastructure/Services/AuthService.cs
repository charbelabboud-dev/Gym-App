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
            // Check if user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == registerDto.Email && !u.IsDeleted);
            
            if (existingUser != null)
                throw new Exception("User with this email already exists");

            // Hash the password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create user object
            var user = new User
            {
                UserEmail = registerDto.Email,
                UserPassword = hashedPassword,
                UserRole = registerDto.Role,
                UserStatus = true,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            // Start a database transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Add user to database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create role-specific record
                if (registerDto.Role == "Client")
                {
                    var client = new Client
                    {
                        ClCode = GenerateClientCode(),
                        UserId = user.UserId,
                        ClFname = registerDto.FullName.Split(' ')[0],
                        ClLname = registerDto.FullName.Split(' ').Length > 1 ? registerDto.FullName.Split(' ')[1] : "",
                        ClBirthDate = DateTime.Now.AddYears(-20),
                        ClPhone = registerDto.Phone ?? "",
                        ClAddress = "",
                        ClRegisterDate = DateTime.Now,
                        ClStatus = true,
                        IsDeleted = false
                    };
                    _context.Clients.Add(client);
                }
                else if (registerDto.Role == "Coach")
                {
                    var coach = new Coach
                    {
                        CoCode = GenerateCoachCode(),
                        UserId = user.UserId,
                        CoFname = registerDto.FullName.Split(' ')[0],
                        CoLname = registerDto.FullName.Split(' ').Length > 1 ? registerDto.FullName.Split(' ')[1] : "",
                        CoBirthDate = DateTime.Now.AddYears(-25),
                        CoPhone = registerDto.Phone ?? "",
                        CoEmail = registerDto.Email,
                        CoAddress = "",
                        CoHireDate = DateTime.Now,
                        CoStatus = true,
                        IsDeleted = false
                    };
                    _context.Coaches.Add(coach);
                }
                else if (registerDto.Role == "Dietitian")
                {
                    var dietitian = new Dietitian
                    {
                        DietCode = GenerateDietitianCode(),
                        UserId = user.UserId,
                        DietFname = registerDto.FullName.Split(' ')[0],
                        DietLname = registerDto.FullName.Split(' ').Length > 1 ? registerDto.FullName.Split(' ')[1] : "",
                        DietEmail = registerDto.Email,
                        DietPhone = registerDto.Phone ?? "",
                        DietStatus = true,
                        IsDeleted = false
                    };
                    _context.Dietitians.Add(dietitian);
                }

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
            // Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == loginDto.Email && !u.IsDeleted);

            if (user == null)
                throw new Exception("Invalid email or password");

            // Verify password using BCrypt
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.UserPassword);
            
            if (!isValidPassword)
                throw new Exception("Invalid email or password");

            if (!user.UserStatus)
                throw new Exception("Account is disabled");

            // Get full name and code based on role
            string fullName = "";
            string clientCode = null;   // ← Add this line
            string coachCode = null;    // ← Add this line
            string dietitianCode = null; // ← Add this line
            
            if (user.UserRole == "Client")
            {
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == user.UserId);
                if (client != null)
                {
                    fullName = client.ClFname + " " + client.ClLname;
                    clientCode = client.ClCode;  // ← Add this line
                }
            }
            else if (user.UserRole == "Coach")
            {
                var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.UserId == user.UserId);
                if (coach != null)
                {
                    fullName = coach.CoFname + " " + coach.CoLname;
                    coachCode = coach.CoCode;  // ← Add this line
                }
            }
            else if (user.UserRole == "Dietitian")
            {
                var dietitian = await _context.Dietitians.FirstOrDefaultAsync(d => d.UserId == user.UserId);
                if (dietitian != null)
                {
                    fullName = dietitian.DietFname + " " + dietitian.DietLname;
                    dietitianCode = dietitian.DietCode;  // ← Add this line
                }
            }

            return new AuthResponseDto
            {
                Token = GenerateJwtToken(user),
                UserId = user.UserId,
                Email = user.UserEmail,
                FullName = fullName,
                Role = user.UserRole,
                ClientCode = clientCode,      // ← Add this line (remove comma if last)
                CoachCode = coachCode,        // ← Add this line (optional)
                DietitianCode = dietitianCode // ← Add this line (optional)
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

        private string GenerateClientCode()
        {
            var lastClient = _context.Clients
                .OrderByDescending(c => c.ClCode)
                .FirstOrDefault();
            
            if (lastClient == null)
                return "CL001";
            
            var number = int.Parse(lastClient.ClCode.Substring(2)) + 1;
            return "CL" + number.ToString("D3");
        }

        private string GenerateCoachCode()
        {
            var lastCoach = _context.Coaches
                .OrderByDescending(c => c.CoCode)
                .FirstOrDefault();
            
            if (lastCoach == null)
                return "CO001";
            
            var number = int.Parse(lastCoach.CoCode.Substring(2)) + 1;
            return "CO" + number.ToString("D3");
        }

        private string GenerateDietitianCode()
        {
            var lastDietitian = _context.Dietitians
                .OrderByDescending(d => d.DietCode)
                .FirstOrDefault();
            
            if (lastDietitian == null)
                return "DT001";
            
            var number = int.Parse(lastDietitian.DietCode.Substring(2)) + 1;
            return "DT" + number.ToString("D3");
        }
    }
}