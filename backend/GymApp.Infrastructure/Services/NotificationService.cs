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
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto dto)
        {
            // Check if user exists
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId && !u.IsDeleted);
            
            if (user == null)
                throw new Exception($"User with ID {dto.UserId} not found");

            var notification = new Notification
            {
                UserId = dto.UserId,
                Message = dto.Message,
                Title = dto.Title,
                Type = dto.Type,
                CreatedDate = DateTime.Now,
                IsRead = false,
                IsDeleted = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return new NotificationDto
            {
                NotId = notification.NotId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type ?? "General",
                IsRead = notification.IsRead,
                CreatedDate = notification.CreatedDate
            };
        }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return notifications.Select(n => new NotificationDto
            {
                NotId = n.NotId,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type ?? "General",
                IsRead = n.IsRead,
                CreatedDate = n.CreatedDate
            }).ToList();
        }

        public async Task<NotificationDto> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotId == notificationId && !n.IsDeleted);

            if (notification == null)
                throw new Exception($"Notification with ID {notificationId} not found");

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return new NotificationDto
            {
                NotId = notification.NotId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type ?? "General",
                IsRead = notification.IsRead,
                CreatedDate = notification.CreatedDate
            };
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsDeleted && !n.IsRead);
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsDeleted && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotId == notificationId && !n.IsDeleted);

            if (notification == null)
                throw new Exception($"Notification with ID {notificationId} not found");

            notification.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}