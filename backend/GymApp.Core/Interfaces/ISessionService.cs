using System.Collections.Generic;
using System.Threading.Tasks;
using GymApp.Core.DTOs;


namespace GymApp.Core.Interfaces
{
    public interface ISessionService 
    {
        Task<SessionDto> BookSessionAsync(CreateSessionDto dto);

        Task<List<SessionDto>> GetClientSessionsAsync(string clientCode);
        
        Task<List<SessionDto>> GetCoachSessionsAsync(string coachCode);
        
        Task<SessionDto> RescheduleSessionAsync(int sessionId , RescheduleSessionDto dto);

        Task<bool> CancelSessionAsync(int sessionId);

        Task<bool> IsTimeSlotAvailableAsync(string coachCode , DateTime dateTime , int duration);

        Task<bool> UpdateSessionStatusAsync(int sessionId, string status);
    }
}