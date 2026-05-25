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
    public class GoalService : IGoalService
    {
        private readonly ApplicationDbContext _context;

        public GoalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GoalDto> CreateGoalAsync(CreateGoalDto dto)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ClCode == dto.ClientCode && !c.IsDeleted);
            
            if (client == null)
                throw new Exception($"Client with code {dto.ClientCode} not found");

            var goal = new ClientGoal
            {
                ClientCode = dto.ClientCode,
                GoalType = dto.GoalType,
                TargetValue = dto.TargetValue,
                TargetDate = dto.TargetDate,
                StartDate = DateTime.Now,
                IsCompleted = false,
                IsDeleted = false
            };

            _context.ClientGoals.Add(goal);
            await _context.SaveChangesAsync();

            return await GetGoalWithProgress(goal);
        }

        public async Task<List<GoalDto>> GetClientGoalsAsync(string clientCode)
        {
            var goals = await _context.ClientGoals
                .Where(g => g.ClientCode == clientCode && !g.IsDeleted)
                .Include(g => g.Client)
                    .ThenInclude(c => c.User)
                .OrderByDescending(g => g.IsCompleted)
                .ThenBy(g => g.TargetDate)
                .ToListAsync();

            var goalDtos = new List<GoalDto>();
            foreach (var goal in goals)
            {
                goalDtos.Add(await GetGoalWithProgress(goal));
            }
            return goalDtos;
        }

        public async Task<GoalDto> UpdateGoalProgressAsync(int goalId, UpdateGoalProgressDto dto)
        {
            var goal = await _context.ClientGoals
                .FirstOrDefaultAsync(g => g.Id == goalId && !g.IsDeleted);

            if (goal == null)
                throw new Exception($"Goal with ID {goalId} not found");

            goal.CurrentValue = dto.CurrentValue;
            
            // Check if goal is completed
            if (!goal.IsCompleted && goal.CurrentValue >= goal.TargetValue)
            {
                goal.IsCompleted = true;
            }

            await _context.SaveChangesAsync();

            return await GetGoalWithProgress(goal);
        }

        public async Task<bool> DeleteGoalAsync(int goalId)
        {
            var goal = await _context.ClientGoals
                .FirstOrDefaultAsync(g => g.Id == goalId && !g.IsDeleted);

            if (goal == null)
                throw new Exception($"Goal with ID {goalId} not found");

            goal.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<GoalDto> GetGoalWithProgress(ClientGoal goal)
        {
            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ClCode == goal.ClientCode);

            int progressPercentage = 0;
            if (goal.CurrentValue.HasValue && goal.TargetValue > 0)
            {
                progressPercentage = (int)((goal.CurrentValue.Value / goal.TargetValue) * 100);
                if (progressPercentage > 100) progressPercentage = 100;
            }

            return new GoalDto
            {
                Id = goal.Id,
                ClientCode = goal.ClientCode,
                ClientName = $"{client.ClFname} {client.ClLname}",
                GoalType = goal.GoalType,
                TargetValue = goal.TargetValue,
                CurrentValue = goal.CurrentValue,
                StartDate = goal.StartDate,
                TargetDate = goal.TargetDate,
                IsCompleted = goal.IsCompleted,
                ProgressPercentage = progressPercentage
            };
        }
    }
}