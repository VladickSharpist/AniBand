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
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Video.Services.Abstractions.Services;
using AutoMapper;

namespace AniBand.Video.Services.Services
{
    internal class VideoService
        : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public VideoService(
            IMapper mapper,
            IFileService fileService,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper
                ?? throw new NullReferenceException(nameof(mapper));
            _fileService = fileService
                ?? throw new NullReferenceException(nameof(fileService));
            _unitOfWork = unitOfWork
                ?? throw new NullReferenceException(nameof(unitOfWork));
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

        public async Task<IHttpResult> SaveVideoAsync(VideoDto videoDto)
        {
            var season = (await _unitOfWork
                    .GetReadonlyRepository<Anime>()
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

        public async Task<IHttpResult<IEnumerable<VideoDto>>> GetVideosBySeasonIdAsync(long id)
        {
            var season = (await _unitOfWork
                .GetReadonlyRepository<Anime>()
                .GetAsync(s =>
                        s.Id == Convert.ToInt64(id)))
                .SingleOrDefault();
            
            var videos = await _unitOfWork
                .GetReadonlyRepository<Episode>()
                .GetAsync(v =>
                        v.SeasonId == Convert.ToInt64(id),
                    null,
                    v => v.Comments
                        .Where(c => 
                            c.Status == Status.Approved),
                    v => v.Rates,
                    v => v.Views);

            if (season == null)
            {
                return new HttpResult<IEnumerable<VideoDto>>(
                    null,
                    "No such Anime",
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
                                c.Status == Status.Approved),
                        v => v.Rates,
                        v => v.Views
                            ))
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
    }
}