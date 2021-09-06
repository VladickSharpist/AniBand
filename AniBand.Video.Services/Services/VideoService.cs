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
using Episode = AniBand.Domain.Models.Video;

namespace AniBand.Video.Services.Services
{
    internal class VideoService
        : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;

        public VideoService(
            IMapper mapper,
            IFileService fileService,
            IUnitOfWork unitOfWork, 
            UserManager<User> userManager, 
            INotificationService notificationService)
        {
            _mapper = mapper
                ?? throw new NullReferenceException(nameof(mapper));
            _fileService = fileService
                ?? throw new NullReferenceException(nameof(fileService));
            _unitOfWork = unitOfWork
                ?? throw new NullReferenceException(nameof(unitOfWork));
            _userManager = userManager
                ?? throw new NullReferenceException(nameof(userManager));
            _notificationService = notificationService
                ?? throw new NullReferenceException(nameof(notificationService));
        }

        public async Task<IHttpResult> AddVideosAsync(
            IEnumerable<VideoDto> videos)
        {
            if (videos == null)
            {
                return new HttpResult(
                    "Files count doesnt match Videos count", 
                    HttpStatusCode.UnprocessableEntity);
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    foreach (var videoDto in videos)
                    {
                        var result = await SaveVideoAsync(videoDto);
                        if (!result.IsSuccessful)
                        {
                            await transaction.RollBackAsync();
                            return result;
                        }
                    }
                    
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

            return new HttpResult();
        }

        public async Task<IHttpResult> AddSeasonAsync(SeasonDto seasonDto)
        {
            if (seasonDto.VideosDto == null)
            {
                return new HttpResult(
                    "Count of Videos lower than count of files",
                    HttpStatusCode.UnprocessableEntity);
            }
            
            var studio = (await _unitOfWork
                    .GetReadonlyRepository<Studio>()
                    .GetNoTrackingAsync(s =>
                        s.Id == seasonDto.StudioId))
                .SingleOrDefault();

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var season = _mapper.Map<Season>(seasonDto);
                    await _unitOfWork
                        .GetReadWriteRepository<Season>()
                        .SaveAsync(season);
            
                    if (seasonDto.VideosDto.Count > 0)
                    {
                        seasonDto.VideosDto
                            .ForEach(v => 
                                v.SeasonId = season.Id);
                        foreach (var videoDto in seasonDto.VideosDto)
                        {
                            var result = await SaveVideoAsync(videoDto);
                            if (!result.IsSuccessful)
                            {
                                await transaction.RollBackAsync();
                                return result;
                            }
                        }
                    }
            
                    var imageFileName = studio != null 
                        ? studio.Id + "-" + season.Id
                        : "NoStudio-" + season.Id;
            
                    season.ImageUrl = _fileService
                        .StoreFileGetUrl(seasonDto.Image, imageFileName);
            
                    await _unitOfWork
                        .GetReadWriteRepository<Season>()
                        .SaveAsync(season);
                    
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

        public async Task<IHttpResult> SaveVideoAsync(VideoDto videoDto)
        {
            var season = (await _unitOfWork
                    .GetReadonlyRepository<Season>()
                    .GetNoTrackingAsync(v =>
                        v.Id == videoDto.SeasonId))
                .SingleOrDefault();

            if (season == null)
            {
                return new HttpResult(
                    "No such season", 
                    HttpStatusCode.BadRequest);
            }

            var video = _mapper.Map<Episode>(videoDto);
            await _unitOfWork
                .GetReadWriteRepository<Episode>()
                .SaveAsync(video);
                
            var fileName = season.Id + "-" + video.Id;
            try
            {
                video.Url = _fileService
                    .StoreFileGetUrl(videoDto.VideoFile, fileName);
                video.DurationInSeconds = _fileService
                    .GetVideoDuration(video.Url);
                video.VideoFileHash = _fileService
                    .GetFileHash(video.Url);
            }
            catch (Exception e)
            {
                return new HttpResult(
                    e.Message,  
                    HttpStatusCode.BadRequest);
            }
                
            await _unitOfWork
                .GetReadWriteRepository<Episode>()
                .SaveAsync(video);

            return new HttpResult();
        }

        public async Task<IHttpResult<IEnumerable<SeasonDto>>> GetAllSeasonsAsync()
        {
            var seasons = await _unitOfWork
                .GetReadonlyRepository<Season>()
                .GetAsync(
                    null,
                    null,
                    season => season.Videos);
            
            var seasonsDto = _mapper.Map<IEnumerable<SeasonDto>>(seasons);

            return new HttpResult<IEnumerable<SeasonDto>>(seasonsDto);
        }

        public async Task<IHttpResult<SeasonDto>> GetSeasonByIdAsync(long id)
        {
            var season = (await _unitOfWork
                .GetReadonlyRepository<Season>()
                .GetAsync(s => 
                    s.Id == Convert.ToInt64(id),
                    null,
                    s => s.Videos))
                .SingleOrDefault();

            if (season == null)
            {
                return new HttpResult<SeasonDto>(
                    null,
                    "No such Season",
                    HttpStatusCode.BadRequest);
            }

            return new HttpResult<SeasonDto>(
                _mapper.Map<SeasonDto>(season));
        }

        public async Task<IHttpResult<IEnumerable<VideoDto>>> GetVideosBySeasonIdAsync(long id)
        {
            var season = (await _unitOfWork
                .GetReadonlyRepository<Season>()
                .GetAsync(s =>
                        s.Id == Convert.ToInt64(id)))
                .SingleOrDefault();
            
            var videos = await _unitOfWork
                .GetReadonlyRepository<Episode>()
                .GetAsync(v =>
                        v.SeasonId == Convert.ToInt64(id),
                    null,
                    v => v.Comments);

            if (season == null)
            {
                return new HttpResult<IEnumerable<VideoDto>>(
                    null,
                    "No such Season",
                    HttpStatusCode.BadRequest);
            }

            var videosDto = _mapper.Map<IEnumerable<VideoDto>>(videos);

            return new HttpResult<IEnumerable<VideoDto>>(videosDto);
        }

        public async Task<IHttpResult<VideoDto>> GetVideoByIdAsync(long id)
        {
            var video = (await _unitOfWork
                    .GetReadonlyRepository<Episode>()
                    .GetAsync(s => 
                        s.Id == Convert.ToInt64(id),
                        null,
                        v => 
                            v.Comments.Where(c => 
                                c.Status == Status.Approved)))
                .SingleOrDefault();

            if (video == null)
            {
                return new HttpResult<VideoDto>(
                    null,
                    "No such Episode",
                    HttpStatusCode.BadRequest);
            }
            
            return new HttpResult<VideoDto>(
                _mapper.Map<VideoDto>(video));
        }

        public async Task<IHttpResult> DeleteSeasonByIdAsync(long id)
        {
            var seasonRepo = _unitOfWork
                .GetReadWriteRepository<Season>();

            var season = (await seasonRepo
                    .GetAsync(s => 
                        s.Id == Convert.ToInt64(id),
                        null,
                        s => s.Videos))
                .SingleOrDefault();

            if (season == null)
            {
                return new HttpResult(
                    "Wrong id or no such season",
                    HttpStatusCode.BadRequest);   
            }
            
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await seasonRepo.RemoveAsync(season);
                    _fileService.DeleteFile(season.ImageUrl);
                    foreach (var seasonVideo in season.Videos)
                    { 
                        _fileService.DeleteFile(seasonVideo.Url);
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

        public async Task<IHttpResult> DeleteVideoByIdAsync(long id)
        {
            var episodeRepo = _unitOfWork
                .GetReadWriteRepository<Episode>();

            var episode = (await episodeRepo
                    .GetAsync(s => 
                        s.Id == Convert.ToInt64(id)))
                .SingleOrDefault();

            if (episode == null)
            {
                return new HttpResult(
                    "Wrong id or no such episode",
                    HttpStatusCode.BadRequest);   
            }
            
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await episodeRepo.RemoveAsync(episode);
                    _fileService.DeleteFile(episode.Url);
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

        public async Task<IHttpResult> AddCommentAsync(CommentDto model)
        {
            var episode = (await _unitOfWork
                    .GetReadWriteRepository<Episode>()
                    .GetAsync(s => 
                        s.Id == Convert.ToInt64(model.VideoId)))
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
                        _notificationService?
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

        public async Task<IHttpResult<IEnumerable<WaitingCommentDto>>> GetAllWaitingCommentsAsync()
        {
            var waitingComments = await _unitOfWork
                .GetReadonlyRepository<Comment>()
                .GetNoTrackingAsync(c =>
                    c.Status == Status.Waiting,
                    null,
                    c => c.User ,
                    c => c.Video);

            var commentsDto = _mapper
                .Map<IEnumerable<WaitingCommentDto>>(waitingComments);

            return new HttpResult<IEnumerable<WaitingCommentDto>>(commentsDto);
        }
    }
}