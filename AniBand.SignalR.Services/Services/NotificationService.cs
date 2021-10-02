using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.Domain.Models;
using AniBand.Query.Services.Abstractions.Services;
using AniBand.SignalR.Services.Abstractions.Hubs;
using AniBand.SignalR.Services.Abstractions.Interfaces;
using AniBand.SignalR.Services.Abstractions.Models;
using AniBand.SignalR.Services.Abstractions.Services;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public NotificationService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager, 
            IHubContext<NotificationHub, IClient> hubContext, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork
                ?? throw new NullReferenceException(nameof(unitOfWork));
            _userManager = userManager
                ?? throw new NullReferenceException(nameof(userManager));
            _hubContext = hubContext
                ?? throw new NullReferenceException(nameof(hubContext));
            _mapper = mapper
                ?? throw new NullReferenceException(nameof(mapper));
        }

        public async Task NotifyAsync(
            string actorId, 
            string message)
        {
            var user = await _userManager.FindByIdAsync(actorId);
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
                .User(actorId)
                .SendNotificationAsync(message);
        }
        
        public async Task<IHttpResult<IEnumerable<NotificationDto>>> GetUnViewedNotificationsAsync(long userId)
        {
            var notificationRepo = _unitOfWork
                .GetReadWriteRepository<Notification>();
            
            var unViewed = await notificationRepo
                .GetAsync(n =>
                    !n.IsViewed);

            var unViewedDto = _mapper.Map<IEnumerable<NotificationDto>>(unViewed);
            
            foreach (var notification in unViewed)
            {
                notification.IsViewed = true;
                await notificationRepo.SaveAsync(notification);
            }
            return new HttpResult<IEnumerable<NotificationDto>>(unViewedDto);
        }
    }
}
