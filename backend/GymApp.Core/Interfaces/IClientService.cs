using System.Collections.Generic;
using System.Threading.Tasks;
using GymApp.Core.DTOs;

namespace GymApp.Core.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetClientsByCoachAsync(string coachCode);
        Task<ClientDto> GetClientDetailsAsync(string clientCode);
        Task<List<ClientDto>> GetClientsByDietitianAsync(string dietitianCode); 
    }
}