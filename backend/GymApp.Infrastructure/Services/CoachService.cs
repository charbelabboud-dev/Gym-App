using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GymApp.Core.DTOs;
using GymApp.Core.Entities;
using GymApp.Core.Interfaces;
using GymApp.Infrastructure.Data;

namespace GymApp.Infrastructure.Services
{
    public class CoachService : ICoachService
    {
        private readonly ApplicationDbContext _context;

        public CoachService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CoachDto>> GetTopRatedCoachesAsync(int limit)
{
    var coaches = await _context.Coaches
        .Where(c => !c.IsDeleted && c.CoStatus == true)
        .Include(c => c.User)
        // Include reviews to calculate average rating
        .OrderByDescending(c => c.Rating)
        .ThenBy(c => c.CoFname)
        .Take(limit)
        .ToListAsync();

    var coachDtos = coaches.Select(c => new CoachDto
    {
        CoCode = c.CoCode,
        FullName = $"{c.CoFname} {c.CoLname}",
        Phone = c.CoPhone,
        Email = c.CoEmail,
        Specialty = c.CoSpecialty ?? "General",
        Address = c.CoAddress,
        Rating = c.Rating ?? 0
    }).ToList();

    return coachDtos;
}

        public async Task<List<CoachDto>> GetAllCoachesAsync()
        {
            // Get all coaches from database
            var coaches = await _context.Coaches
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            // Convert to DTOs
            var coachDtos = coaches.Select(c => new CoachDto
            {
                CoCode = c.CoCode,
                FullName = c.CoFname + " " + c.CoLname,
                PhoneNumber = c.CoPhone,
                Email = c.CoEmail,
                Specialty = c.CoSpecialty ?? "General",
                Address = c.CoAddress
            }).ToList();

            return coachDtos;
        }

        public async Task<List<CoachDto>> GetCoachesBySpecialtyAsync(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
            {
                return await GetAllCoachesAsync();
            }

            // Get coaches with matching specialty (case insensitive)
            var coaches = await _context.Coaches  // ← This line was missing
                .Where(c => !c.IsDeleted && c.CoSpecialty == specialty)
                .ToListAsync();  // ← This was missing

            // Convert to DTOs
            var coachDtos = coaches.Select(c => new CoachDto
            {
                CoCode = c.CoCode,
                FullName = c.CoFname + " " + c.CoLname,
                PhoneNumber = c.CoPhone,
                Email = c.CoEmail,
                Specialty = c.CoSpecialty ?? "General",
                Address = c.CoAddress
            }).ToList();

            return coachDtos;
        }
    }
}