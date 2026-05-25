using System.Collections.Generic;
using System.Threading.Tasks;
using GymApp.Core.DTOs;

namespace GymApp.Core.Interfaces
{
    public interface IGoalService
    {
        Task<GoalDto> CreateGoalAsync(CreateGoalDto dto);
        Task<List<GoalDto>> GetClientGoalsAsync(string clientCode);
        Task<GoalDto> UpdateGoalProgressAsync(int goalId, UpdateGoalProgressDto dto);
        Task<bool> DeleteGoalAsync(int goalId);
    }
}