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
    public class ProgressService : IProgressService
    {
        private readonly ApplicationDbContext _context;

        public ProgressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProgressDto> AddProgressAsync(CreateProgressDto dto)
        {
            // Check if client exists
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ClCode == dto.ClientCode && !c.IsDeleted);
            
            if (client == null)
                throw new Exception($"Client with code {dto.ClientCode} not found");

            var progress = new ProgressEntry
            {
                ClientId = client.UserId,
                Weight = dto.Weight,
                BodyFatPercentage = dto.BodyFatPercentage,
                Chest = dto.Chest,
                Waist = dto.Waist,
                Notes = dto.Notes,
                EntryDate = dto.EntryDate,
                IsDeleted = false
            };

            _context.ProgressEntries.Add(progress);
            await _context.SaveChangesAsync();

            return new ProgressDto
            {
                Id = progress.Id,
                ClientName = $"{client.ClFname} {client.ClLname}",
                Weight = progress.Weight,
                BodyFatPercentage = progress.BodyFatPercentage,
                Chest = progress.Chest,
                Waist = progress.Waist,
                Notes = progress.Notes,
                EntryDate = progress.EntryDate
            };
        }

        public async Task<List<ProgressDto>> GetClientProgressAsync(string clientCode)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.ClCode == clientCode && !c.IsDeleted);
            
            if (client == null)
                throw new Exception($"Client with code {clientCode} not found");

            var progress = await _context.ProgressEntries
                .Where(p => p.ClientId == client.UserId && !p.IsDeleted)
                .OrderByDescending(p => p.EntryDate)
                .ToListAsync();

            return progress.Select(p => new ProgressDto
            {
                Id = p.Id,
                ClientName = $"{client.ClFname} {client.ClLname}",
                Weight = p.Weight,
                BodyFatPercentage = p.BodyFatPercentage,
                Chest = p.Chest,
                Waist = p.Waist,
                Notes = p.Notes,
                EntryDate = p.EntryDate
            }).ToList();
        }

        public async Task<ProgressDto> GetProgressByIdAsync(int id)
        {
            var progress = await _context.ProgressEntries
                .Include(p => p.Client)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (progress == null)
                throw new Exception($"Progress entry with ID {id} not found");

            return new ProgressDto
            {
                Id = progress.Id,
                ClientName = $"{progress.Client.ClFname} {progress.Client.ClLname}",
                Weight = progress.Weight,
                BodyFatPercentage = progress.BodyFatPercentage,
                Chest = progress.Chest,
                Waist = progress.Waist,
                Notes = progress.Notes,
                EntryDate = progress.EntryDate
            };
        }

        public async Task<bool> DeleteProgressAsync(int id)
        {
            var progress = await _context.ProgressEntries
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (progress == null)
                throw new Exception($"Progress entry with ID {id} not found");

            progress.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}