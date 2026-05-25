using System;
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
    public class DietPlanService : IDietPlanService
    {
        private readonly ApplicationDbContext _context;

        public DietPlanService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<DietPlanDto> CreateDietPlanAsync(CreateDietPlanDto dto)
        {
            // Check if client exists
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ClCode == dto.ClientCode && !c.IsDeleted);
            
            if (client == null)
                throw new Exception($"Client with code {dto.ClientCode} not found");

            // Check if dietitian exists
            var dietitian = await _context.Dietitians
                .FirstOrDefaultAsync(d => d.DietCode == dto.DietitianCode && !d.IsDeleted);
            
            if (dietitian == null)
                throw new Exception($"Dietitian with code {dto.DietitianCode} not found");

            // Calculate duration in days
            int durationDays = (int)(dto.EndDate - dto.StartDate).TotalDays;

            // Create diet plan
            var dietPlan = new DietPlan
            {
                ClCode = dto.ClientCode,
                DietitianId = dto.DietitianCode,
                DietDescription = dto.Description,
                DietStartDate = dto.StartDate,
                DietEndDate = dto.EndDate,
                IsDeleted = false
            };

            _context.DietPlans.Add(dietPlan);
            await _context.SaveChangesAsync();

            return await GetPlanByIdAsync(dietPlan.DietId);
        }

        public async Task<List<DietPlanDto>> GetClientPlansAsync(string clientCode)
        {
            var plans = await _context.DietPlans
                .Where(p => p.ClCode == clientCode && !p.IsDeleted)
                .Include(p => p.Client)
                    .ThenInclude(c => c.User)
                .Include(p => p.Dietitian)
                    .ThenInclude(d => d.User)
                .OrderByDescending(p => p.DietStartDate)
                .ToListAsync();

            return plans.Select(p => MapToDto(p)).ToList();
        }

        public async Task<List<DietPlanDto>> GetDietitianPlansAsync(string dietitianCode)
        {
            var plans = await _context.DietPlans
                .Where(p => p.DietitianId == dietitianCode && !p.IsDeleted)
                .Include(p => p.Client)
                    .ThenInclude(c => c.User)
                .Include(p => p.Dietitian)
                    .ThenInclude(d => d.User)
                .OrderByDescending(p => p.DietStartDate)
                .ToListAsync();

            return plans.Select(p => MapToDto(p)).ToList();
        }

        public async Task<DietPlanDto> GetPlanByIdAsync(int planId)
        {
            var plan = await _context.DietPlans
                .Include(p => p.Client)
                    .ThenInclude(c => c.User)
                .Include(p => p.Dietitian)
                    .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(p => p.DietId == planId && !p.IsDeleted);

            if (plan == null)
                throw new Exception($"Diet plan with ID {planId} not found");

            return MapToDto(plan);
        }

        public async Task<DietPlanDto> UpdatePlanAsync(int planId, UpdateDietPlanDto dto)
        {
            var plan = await _context.DietPlans
                .FirstOrDefaultAsync(p => p.DietId == planId && !p.IsDeleted);

            if (plan == null)
                throw new Exception($"Diet plan with ID {planId} not found");

            plan.DietDescription = dto.Description;
            plan.DietStartDate = dto.StartDate;
            plan.DietEndDate = dto.EndDate;

            await _context.SaveChangesAsync();

            return await GetPlanByIdAsync(planId);
        }

        public async Task<bool> DeletePlanAsync(int planId)
        {
            var plan = await _context.DietPlans
                .FirstOrDefaultAsync(p => p.DietId == planId && !p.IsDeleted);

            if (plan == null)
                throw new Exception($"Diet plan with ID {planId} not found");

            plan.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        private DietPlanDto MapToDto(DietPlan plan)
        {
            return new DietPlanDto
            {
                DietId = plan.DietId,
                ClientName = $"{plan.Client.ClFname} {plan.Client.ClLname}",
                DietitianName = $"{plan.Dietitian.DietFname} {plan.Dietitian.DietLname}",
                Description = plan.DietDescription,
                StartDate = plan.DietStartDate,
                EndDate = plan.DietEndDate,
            };
        }
    }
}