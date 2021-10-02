using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.Domain.Enums;
using AniBand.Domain.Models;
using AniBand.SignalR.Services.Abstractions.Services;
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Video.Services.Abstractions.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Video.Services.Services
{
    public class CommentService
        : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;

        public CommentService(
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            UserManager<User> userManager, 
            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<IHttpResult> AddCommentAsync(CommentDto model)
        {
            var episode = (await _unitOfWork
                    .GetReadWriteRepository<Episode>()
                    .GetAsync(s => 
                        s.Id == model.VideoId))
                .SingleOrDefault();

            if (episode == null)
            {
                return new HttpResult(
                    "Wrong id or no such episode",
                    HttpStatusCode.BadRequest);   
            }

            var user = await _userManager
                .FindByIdAsync(model.UserId.ToString());
            var admins = await _userManager
                .GetUsersInRoleAsync(Roles.Admin.ToString());
            
            if (user == null)
            {
                return new HttpResult(
                    "Wrong id or no such user",
                    HttpStatusCode.BadRequest);  
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var comment = _mapper.Map<Comment>(model);
                    await _unitOfWork
                        .GetReadWriteRepository<Comment>()
                        .SaveAsync(comment);
                    foreach (var admin in admins)
                    {
                        await _notificationService
                            .NotifyAsync(
                                admin.Id.ToString(), 
                                $"New comment from {user.Email}");
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollBackAsync();
                    return new HttpResult(
                        e.Message,
                        HttpStatusCode.BadRequest);
                }
            }

            return new HttpResult();
        }

        public async Task<IHttpResult> ApproveCommentAsync(long id)
        {
            var commentRepo = _unitOfWork
                .GetReadWriteRepository<Comment>();
            var comment = (await commentRepo
                    .GetAsync(c => c.Id == id))
                .SingleOrDefault();
            if (comment == null)
            {
                return new HttpResult(
                    "Wrong id",
                    HttpStatusCode.UnprocessableEntity);
            }
            
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    comment.Status = Status.Approved;
                    await commentRepo.SaveAsync(comment);
                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollBackAsync();
                    return new HttpResult(
                        e.Message,
                        HttpStatusCode.UnprocessableEntity);
                }
            }
            
            var user = await _userManager
                .FindByIdAsync(comment.UserId.ToString());
            await _notificationService.NotifyAsync(
                user.Id.ToString(),
                "Your comment approved");

            return new HttpResult();
        }
        
        public async Task<IHttpResult> DeclineCommentAsync(long id, string declineMessage)
        {
            var commentRepo = _unitOfWork
                .GetReadWriteRepository<Comment>();
            var comment = (await commentRepo
                    .GetAsync(c => c.Id == id))
                .SingleOrDefault();
            if (comment == null)
            {
                return new HttpResult(
                    "Wrong id",
                    HttpStatusCode.UnprocessableEntity);
            }
            
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    comment.Status = Status.Declined;
                    comment.DeclineMessage = declineMessage;
                    await commentRepo.SaveAsync(comment);
                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollBackAsync();
                    return new HttpResult(
                        e.Message,
                        HttpStatusCode.UnprocessableEntity);
                }
            }
            
            var user = await _userManager
                .FindByIdAsync(comment.UserId.ToString());
            await _notificationService.NotifyAsync(
                user.Id.ToString(),
                "Your comment declined");

            return new HttpResult();
        }
    }
}