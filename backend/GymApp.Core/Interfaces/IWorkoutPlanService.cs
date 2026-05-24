using System.Collections.Generic;
using System.Threading.Tasks;
using GymApp.Core.DTOs;

namespace GymApp.Core.Interfaces
{
    public interface IWorkoutPlanService
    {
        // Create a new workout plan with exercises
        Task<WorkoutPlanDto> CreateWorkoutPlanAsync(CreateWorkoutPlanDto dto);
        
        // Get all plans for a specific client
        Task<List<WorkoutPlanDto>> GetClientPlansAsync(string clientCode);
        
        // Get all plans created by a specific coach
        Task<List<WorkoutPlanDto>> GetCoachPlansAsync(string coachCode);
        
        // Get a single plan by ID with all exercises
        Task<WorkoutPlanDto> GetPlanByIdAsync(int planId);
        
        // Update plan details (name, dates)
        Task<WorkoutPlanDto> UpdatePlanAsync(int planId, UpdateWorkoutPlanDto dto);
        
        // Delete a plan (soft delete)
        Task<bool> DeletePlanAsync(int planId);
        
        // Add an exercise to existing plan
        Task<ExerciseDto> AddExerciseToPlanAsync(int planId, CreateExerciseDto dto);
        
        // Remove an exercise from plan
        Task<bool> RemoveExerciseAsync(int exerciseId);
    }
}