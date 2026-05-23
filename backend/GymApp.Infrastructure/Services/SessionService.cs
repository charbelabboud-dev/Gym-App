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
    public class SessionService : ISessionService
    {
        private readonly ApplicationDbContext _context;

        public SessionService(ApplicationDbContext context)
        {
            _context = context;
        }

public async Task<SessionDto> BookSessionAsync(CreateSessionDto dto)
{
    // Check client exists
    var client = await _context.Clients
        .FirstOrDefaultAsync(c => c.ClCode == dto.ClientCode && !c.IsDeleted);

    if (client == null)
        throw new Exception($"Client with code {dto.ClientCode} not found");

    // Check coach exists
    var coach = await _context.Coaches
        .FirstOrDefaultAsync(c => c.CoCode == dto.CoachCode && !c.IsDeleted);

    if (coach == null)
        throw new Exception($"Coach with code {dto.CoachCode} not found");

    // Check availability
    bool isAvailable = await IsTimeSlotAvailableAsync(dto.CoachCode, dto.Date, dto.Duration);

    if (!isAvailable)
        throw new Exception("This time slot is already booked");

    // Create session entity
    var session = new Session
    {
        SesClCode = dto.ClientCode,
        SesCoCode = dto.CoachCode,
        SesType = dto.SessionType,
        SesDescription = dto.Description,
        SesDateTime = dto.Date,
        SesDuration = dto.Duration,
        SesStatus = "Scheduled",
        IsDeleted = false
    };

    // Save to database
    _context.Sessions.Add(session);
    await _context.SaveChangesAsync();

    // ============================================
    // CREATE NOTIFICATIONS
    // ============================================
    
    // Notification for client
    var clientNotification = new Notification
    {
        UserId = client.UserId,
        Title = "Session Booked",
        Message = $"Your session with {coach.CoFname} {coach.CoLname} on {dto.Date:dd/MM/yyyy HH:mm} has been booked.",
        Type = "Session",
        CreatedDate = DateTime.Now,
        IsRead = false,
        IsDeleted = false
    };
    _context.Notifications.Add(clientNotification);
    
    // Notification for coach
    var coachNotification = new Notification
    {
        UserId = coach.UserId,
        Title = "New Session Request",
        Message = $"New session request from {client.ClFname} {client.ClLname} on {dto.Date:dd/MM/yyyy HH:mm}.",
        Type = "Session",
        CreatedDate = DateTime.Now,
        IsRead = false,
        IsDeleted = false
    };
    _context.Notifications.Add(coachNotification);
    
    await _context.SaveChangesAsync();

    // Get client with user for name
    var clientWithUser = await _context.Clients
        .Include(c => c.User)
        .FirstOrDefaultAsync(c => c.ClCode == dto.ClientCode);

    // Get coach with user for name
    var coachWithUser = await _context.Coaches
        .Include(c => c.User)
        .FirstOrDefaultAsync(c => c.CoCode == dto.CoachCode);

    // Return DTO
    return new SessionDto
    {
        SesId = session.SesId,
        ClientName = $"{clientWithUser.ClFname} {clientWithUser.ClLname}",
        CoachName = $"{coachWithUser.CoFname} {coachWithUser.CoLname}",
        SessionType = session.SesType,
        Description = session.SesDescription,
        Date = session.SesDateTime,
        Duration = session.SesDuration,
        Status = session.SesStatus
    };
}
        public async Task<List<SessionDto>> GetClientSessionsAsync(string clientCode)
        {
            var sessions = await _context.Sessions
                .Where(s => s.SesClCode == clientCode && !s.IsDeleted)
                .Include(s => s.Coach)
                    .ThenInclude(c => c.User)
                .Include(s => s.Client)
                    .ThenInclude(c => c.User)
                .OrderByDescending(s => s.SesDateTime)
                .ToListAsync();

            var sessionDtos = sessions.Select(s => new SessionDto
            {
                SesId = s.SesId,
                ClientName = $"{s.Client.ClFname} {s.Client.ClLname}",
                CoachName = $"{s.Coach.CoFname} {s.Coach.CoLname}",
                SessionType = s.SesType,
                Description = s.SesDescription,
                Date = s.SesDateTime,
                Duration = s.SesDuration,
                Status = s.SesStatus
            }).ToList();

            return sessionDtos;
        }

        public async Task<List<SessionDto>> GetCoachSessionsAsync(string coachCode)
        {
            var sessions = await _context.Sessions
                .Where(s => s.SesCoCode == coachCode && !s.IsDeleted)
                .Include(s => s.Client)
                    .ThenInclude(c => c.User)
                .Include(s => s.Coach)
                    .ThenInclude(c => c.User)
                .OrderByDescending(s => s.SesDateTime)
                .ToListAsync();

            var sessionDtos = sessions.Select(s => new SessionDto
            {
                SesId = s.SesId,
                ClientName = $"{s.Client.ClFname} {s.Client.ClLname}",
                CoachName = $"{s.Coach.CoFname} {s.Coach.CoLname}",
                SessionType = s.SesType,
                Description = s.SesDescription,
                Date = s.SesDateTime,
                Duration = s.SesDuration,
                Status = s.SesStatus
            }).ToList();

            return sessionDtos;
        }

public async Task<bool> UpdateSessionStatusAsync(int sessionId, string status)
{
    var session = await _context.Sessions
        .Include(s => s.Client)
        .FirstOrDefaultAsync(s => s.SesId == sessionId && !s.IsDeleted);

    if (session == null)
        throw new Exception($"Session with ID {sessionId} not found");

    if (status != "Completed" && status != "Cancelled")
        throw new Exception("Invalid status. Allowed values: Completed, Cancelled");

    session.SesStatus = status;
    await _context.SaveChangesAsync();

    // Create notification for client
    var clientNotification = new Notification
    {
        UserId = session.Client.UserId,
        Title = status == "Completed" ? "Session Completed" : "Session Cancelled",
        Message = status == "Completed" 
            ? $"Your session on {session.SesDateTime:dd/MM/yyyy HH:mm} has been marked as completed."
            : $"Your session on {session.SesDateTime:dd/MM/yyyy HH:mm} has been cancelled by coach.",
        Type = "Session",
        CreatedDate = DateTime.Now,
        IsRead = false,
        IsDeleted = false
    };
    _context.Notifications.Add(clientNotification);
    await _context.SaveChangesAsync();

    return true;
}
public async Task<SessionDto> RescheduleSessionAsync(int sessionId, RescheduleSessionDto dto)
{
    // Find the session
    var session = await _context.Sessions
        .Include(s => s.Client)
            .ThenInclude(c => c.User)
        .Include(s => s.Coach)
            .ThenInclude(c => c.User)
        .FirstOrDefaultAsync(s => s.SesId == sessionId && !s.IsDeleted);

    if (session == null)
        throw new Exception($"Session with ID {sessionId} not found");

    if (session.SesStatus == "Cancelled")
        throw new Exception("Cannot reschedule a cancelled session");

    if (session.SesStatus == "Completed")
        throw new Exception("Cannot reschedule a completed session");

    // Check availability for new time slot
    bool isAvailable = await IsTimeSlotAvailableAsync(session.SesCoCode, dto.Date, dto.Duration);

    if (!isAvailable)
        throw new Exception("The new time slot is already booked");

    // Store old date for notification
    var oldDate = session.SesDateTime;

    // Update session
    session.SesDateTime = dto.Date;
    session.SesDuration = dto.Duration;
    session.SesStatus = "Scheduled";

    await _context.SaveChangesAsync();

    // ============================================
    // CREATE NOTIFICATION
    // ============================================
    
    var rescheduleNotification = new Notification
    {
        UserId = session.Client.UserId,
        Title = "Session Rescheduled",
        Message = $"Your session has been rescheduled from {oldDate:dd/MM/yyyy HH:mm} to {dto.Date:dd/MM/yyyy HH:mm}.",
        Type = "Session",
        CreatedDate = DateTime.Now,
        IsRead = false,
        IsDeleted = false
    };
    _context.Notifications.Add(rescheduleNotification);
    await _context.SaveChangesAsync();

    // Return DTO
    return new SessionDto
    {
        SesId = session.SesId,
        ClientName = $"{session.Client.ClFname} {session.Client.ClLname}",
        CoachName = $"{session.Coach.CoFname} {session.Coach.CoLname}",
        SessionType = session.SesType,
        Description = session.SesDescription,
        Date = session.SesDateTime,
        Duration = session.SesDuration,
        Status = session.SesStatus
    };
}
       public async Task<bool> CancelSessionAsync(int sessionId)
{
    var session = await _context.Sessions
        .Include(s => s.Client)
            .ThenInclude(c => c.User)
        .FirstOrDefaultAsync(s => s.SesId == sessionId && !s.IsDeleted);

    if (session == null)
        throw new Exception($"Session with ID {sessionId} not found");

    if (session.SesStatus == "Cancelled")
        throw new Exception("Session is already cancelled");

    if (session.SesStatus == "Completed")
        throw new Exception("Cannot cancel a completed session");

    session.SesStatus = "Cancelled";
    await _context.SaveChangesAsync();

    // ============================================
    // CREATE NOTIFICATION
    // ============================================
    
    var cancelNotification = new Notification
    {
        UserId = session.Client.UserId,
        Title = "Session Cancelled",
        Message = $"Your session on {session.SesDateTime:dd/MM/yyyy HH:mm} has been cancelled.",
        Type = "Session",
        CreatedDate = DateTime.Now,
        IsRead = false,
        IsDeleted = false
    };
    _context.Notifications.Add(cancelNotification);
    await _context.SaveChangesAsync();

    return true;
}

        public async Task<bool> IsTimeSlotAvailableAsync(string coachCode, DateTime Date, int duration)
        {
            DateTime endTime = Date.AddMinutes(duration);

            var hasOverlap = await _context.Sessions
                .Where(s => s.SesCoCode == coachCode
                            && !s.IsDeleted
                            && s.SesStatus != "Cancelled"
                            && s.SesDateTime < endTime
                            && s.SesDateTime.AddMinutes(s.SesDuration) > Date)
                .AnyAsync();

            return !hasOverlap;
        }
    }
}