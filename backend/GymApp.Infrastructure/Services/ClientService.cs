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
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

public async Task<List<ClientDto>> GetClientsByDietitianAsync(string dietitianCode)
{
    var clients = await _context.Clients
        .Where(c => c.DietitianId == dietitianCode && !c.IsDeleted)
        .Include(c => c.User)
        .Include(c => c.Sessions)
        .ToListAsync();

    var clientDtos = clients.Select(c => new ClientDto
    {
        ClCode = c.ClCode,
        FullName = $"{c.ClFname} {c.ClLname}",
        Email = c.User?.UserEmail ?? "",
        Phone = c.ClPhone,
        RegisterDate = c.ClRegisterDate,
        TotalSessions = c.Sessions?.Count ?? 0,
        CompletedSessions = c.Sessions?.Count(s => s.SesStatus == "Completed") ?? 0
    }).ToList();

    return clientDtos;
}
public async Task<ClientDto> GetClientDetailsAsync(string clientCode)
{
    var client = await _context.Clients
        .Where(c => c.ClCode == clientCode && !c.IsDeleted)
        .Include(c => c.User)
        .Include(c => c.Sessions)
        .FirstOrDefaultAsync();

    if (client == null)
        throw new Exception($"Client with code {clientCode} not found");

    return new ClientDto
    {
        ClCode = client.ClCode,
        FullName = $"{client.ClFname} {client.ClLname}",
        Email = client.User?.UserEmail ?? "",
        Phone = client.ClPhone,
        RegisterDate = client.ClRegisterDate,
        TotalSessions = client.Sessions?.Count ?? 0,
        CompletedSessions = client.Sessions?.Count(s => s.SesStatus == "Completed") ?? 0
    };
}
        public async Task<List<ClientDto>> GetClientsByCoachAsync(string coachCode)
        {
            var clients = await _context.Clients
                .Where(c => c.ClCoachId == coachCode && !c.IsDeleted)
                .Include(c => c.User)
                .Include(c => c.Sessions)
                .ToListAsync();

            var clientDtos = clients.Select(c => new ClientDto
            {
                ClCode = c.ClCode,
                FullName = $"{c.ClFname} {c.ClLname}",
                Email = c.User?.UserEmail ?? "",
                Phone = c.ClPhone,
                RegisterDate = c.ClRegisterDate,
                TotalSessions = c.Sessions?.Count ?? 0,
                CompletedSessions = c.Sessions?.Count(s => s.SesStatus == "Completed") ?? 0
            }).ToList();

            return clientDtos;
        }
    }
}