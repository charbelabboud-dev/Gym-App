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
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkoutPlanDto> CreateWorkoutPlanAsync(CreateWorkoutPlanDto dto)
        {
            // Check if client exists
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ClCode == dto.ClientCode && !c.IsDeleted);
            
            if (client == null)
                throw new Exception($"Client with code {dto.ClientCode} not found");

            // Check if coach exists
            var coach = await _context.Coaches
                .FirstOrDefaultAsync(c => c.CoCode == dto.CoachCode && !c.IsDeleted);
            
            if (coach == null)
                throw new Exception($"Coach with code {dto.CoachCode} not found");

            // Create the workout plan
            var workoutPlan = new WorkoutPlan
            {
                ClCode = dto.ClientCode,
                CoCode = dto.CoachCode,
                WpName = dto.PlanName,
                WpStartDate = dto.StartDate,
                WpEndDate = dto.EndDate,
                IsDeleted = false
            };

            _context.WorkoutPlans.Add(workoutPlan);
            await _context.SaveChangesAsync();

            // Add exercises
            var exercises = new List<WorkoutExercise>();
            foreach (var exerciseDto in dto.Exercises)
            {
                var exercise = new WorkoutExercise
                {
                    WpId = workoutPlan.WpId,
                    ExerciseName = exerciseDto.ExerciseName,
                    Sets = exerciseDto.Sets,
                    Reps = exerciseDto.Reps,
                    RestSeconds = exerciseDto.RestSeconds,
                    IsDeleted = false
                };
                exercises.Add(exercise);
            }

            _context.WorkoutExercises.AddRange(exercises);
            await _context.SaveChangesAsync();

            // Return the created plan with exercises
            return await GetPlanByIdAsync(workoutPlan.WpId);
        }

        public async Task<List<WorkoutPlanDto>> GetClientPlansAsync(string clientCode)
        {
            var plans = await _context.WorkoutPlans
                .Where(p => p.ClCode == clientCode && !p.IsDeleted)
                .Include(p => p.Client)
                    .ThenInclude(c => c.User)
                .Include(p => p.Coach)
                    .ThenInclude(c => c.User)
                .Include(p => p.WorkoutExercises)
                .OrderByDescending(p => p.WpStartDate)
                .ToListAsync();

            return plans.Select(p => MapToDto(p)).ToList();
        }

        public async Task<List<WorkoutPlanDto>> GetCoachPlansAsync(string coachCode)
        {
            var plans = await _context.WorkoutPlans
                .Where(p => p.CoCode == coachCode && !p.IsDeleted)
                .Include(p => p.Client)
                    .ThenInclude(c => c.User)
                .Include(p => p.Coach)
                    .ThenInclude(c => c.User)
                .Include(p => p.WorkoutExercises)
                .OrderByDescending(p => p.WpStartDate)
                .ToListAsync();

            return plans.Select(p => MapToDto(p)).ToList();
        }

        public async Task<WorkoutPlanDto> GetPlanByIdAsync(int planId)
        {
            var plan = await _context.WorkoutPlans
                .Include(p => p.Client)
                    .ThenInclude(c => c.User)
                .Include(p => p.Coach)
                    .ThenInclude(c => c.User)
                .Include(p => p.WorkoutExercises)
                .FirstOrDefaultAsync(p => p.WpId == planId && !p.IsDeleted);

            if (plan == null)
                throw new Exception($"Workout plan with ID {planId} not found");

            return MapToDto(plan);
        }

        public async Task<WorkoutPlanDto> UpdatePlanAsync(int planId, UpdateWorkoutPlanDto dto)
        {
            var plan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(p => p.WpId == planId && !p.IsDeleted);

            if (plan == null)
                throw new Exception($"Workout plan with ID {planId} not found");

            plan.WpName = dto.PlanName;
            plan.WpStartDate = dto.StartDate;
            plan.WpEndDate = dto.EndDate;

            await _context.SaveChangesAsync();

            return await GetPlanByIdAsync(planId);
        }

        public async Task<bool> DeletePlanAsync(int planId)
        {
            var plan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(p => p.WpId == planId && !p.IsDeleted);

            if (plan == null)
                throw new Exception($"Workout plan with ID {planId} not found");

            plan.IsDeleted = true;
            
            // Also soft delete all exercises
            var exercises = await _context.WorkoutExercises
                .Where(e => e.WpId == planId && !e.IsDeleted)
                .ToListAsync();
            
            foreach (var exercise in exercises)
            {
                exercise.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ExerciseDto> AddExerciseToPlanAsync(int planId, CreateExerciseDto dto)
        {
            var plan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(p => p.WpId == planId && !p.IsDeleted);

            if (plan == null)
                throw new Exception($"Workout plan with ID {planId} not found");

            var exercise = new WorkoutExercise
            {
                WpId = planId,
                ExerciseName = dto.ExerciseName,
                Sets = dto.Sets,
                Reps = dto.Reps,
                RestSeconds = dto.RestSeconds,
                IsDeleted = false
            };

            _context.WorkoutExercises.Add(exercise);
            await _context.SaveChangesAsync();

            return new ExerciseDto
            {
                WeId = exercise.WeId,
                ExerciseName = exercise.ExerciseName,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                RestSeconds = exercise.RestSeconds
            };
        }

        public async Task<bool> RemoveExerciseAsync(int exerciseId)
        {
            var exercise = await _context.WorkoutExercises
                .FirstOrDefaultAsync(e => e.WeId == exerciseId && !e.IsDeleted);

            if (exercise == null)
                throw new Exception($"Exercise with ID {exerciseId} not found");

            exercise.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        private WorkoutPlanDto MapToDto(WorkoutPlan plan)
        {
            return new WorkoutPlanDto
            {
                WpId = plan.WpId,
                ClientName = $"{plan.Client.ClFname} {plan.Client.ClLname}",
                CoachName = $"{plan.Coach.CoFname} {plan.Coach.CoLname}",
                PlanName = plan.WpName,
                StartDate = plan.WpStartDate,
                EndDate = plan.WpEndDate,
                Exercises = plan.WorkoutExercises
                    .Where(e => !e.IsDeleted)
                    .Select(e => new ExerciseDto
                    {
                        WeId = e.WeId,
                        ExerciseName = e.ExerciseName,
                        Sets = e.Sets,
                        Reps = e.Reps,
                        RestSeconds = e.RestSeconds
                    }).ToList()
            };
        }
    }
}