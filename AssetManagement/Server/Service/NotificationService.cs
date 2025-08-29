using AssetManagement.DataContext;
using AssetManagement.Dto.Models;
using AssetManagement.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Server.Service
{
    public class NotificationService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<Notification> AddNotification(string message)
        {
            var notification = new Notification
            {
                Message = message,
                CreatedOn = DateTime.UtcNow,
                IsRead = false
            };
            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
            return notification;
        }

        public Task<List<Notification>> GetNotifications()
        {
            return _context.Notification
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();
        }
    }
}
