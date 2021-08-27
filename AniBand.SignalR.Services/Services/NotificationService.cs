using System;
using System.Threading.Tasks;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.Domain.Models;
using AniBand.SignalR.Services.Abstractions.Hubs;
using AniBand.SignalR.Services.Abstractions.Interfaces;
using AniBand.SignalR.Services.Abstractions.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace AniBand.SignalR.Services.Services
{
    internal class NotificationService 
        : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<NotificationHub, IClient> _hubContext;

        public NotificationService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager, 
            IHubContext<NotificationHub, IClient> hubContext)
        {
            _unitOfWork = unitOfWork
               ?? throw new NullReferenceException(nameof(unitOfWork));
            _userManager = userManager
               ?? throw new NullReferenceException(nameof(userManager));
            _hubContext = hubContext
               ?? throw new NullReferenceException(nameof(hubContext));
        }

        public async Task NotifyAsync(
            string actor, 
            string message)
        {
            var user = await _userManager.FindByEmailAsync(actor);
            if (user == null)
            {
                return;
            }

            await _unitOfWork
                .GetReadWriteRepository<Notification>()
                .SaveAsync(new Notification
                {
                    UserId = user.Id,
                    Message = message
                });

            await _hubContext
                .Clients
                .User(actor)
                .SendNotificationAsync(message);
        }
    }
}