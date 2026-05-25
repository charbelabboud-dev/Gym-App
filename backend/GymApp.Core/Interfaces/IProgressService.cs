using System.Collections.Generic;
using System.Threading.Tasks;
using GymApp.Core.DTOs;

namespace GymApp.Core.Interfaces
{
    public interface IProgressService
    {
        Task<ProgressDto> AddProgressAsync(CreateProgressDto dto);
        Task<List<ProgressDto>> GetClientProgressAsync(string clientCode);
        Task<ProgressDto> GetProgressByIdAsync(int id);
        Task<bool> DeleteProgressAsync(int id);
    }
}