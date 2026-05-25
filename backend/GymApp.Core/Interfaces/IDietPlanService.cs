using System.Collections.Generic;
using System.Threading.Tasks;
using GymApp.Core.DTOs;

namespace GymApp.Core.Interfaces
{
    public interface IDietPlanService
    {
        Task<DietPlanDto> CreateDietPlanAsync(CreateDietPlanDto dto);
        Task<List<DietPlanDto>> GetClientPlansAsync(string clientCode);
        Task<List<DietPlanDto>> GetDietitianPlansAsync(string dietitianCode);
        Task<DietPlanDto> GetPlanByIdAsync(int planId);
        Task<DietPlanDto> UpdatePlanAsync(int planId, UpdateDietPlanDto dto);
        Task<bool> DeletePlanAsync(int planId);
    }
}