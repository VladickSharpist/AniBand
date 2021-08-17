using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Infrastructure.Helpers;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.Domain.Models;
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Video.Services.Abstractions.Services;
using AutoMapper;

using Episode = AniBand.Domain.Models.Video;

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

        public async Task<IHttpResult> AddVideoAsync(IEnumerable<VideoDto> videos)
        {
            if (videos == null)
            {
                return new HttpResult(
                    "Files count doesnt match Videos count", 
                    HttpStatusCode.UnprocessableEntity);
            }
            
            foreach (var videoDto in videos)
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
                    await _unitOfWork
                        .GetReadWriteRepository<Episode>()
                        .RemoveAsync(video);
                    return new HttpResult(
                        e.Message,  
                        HttpStatusCode.BadRequest);
                }
                
                await _unitOfWork
                    .GetReadWriteRepository<Episode>()
                    .SaveAsync(video);
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
            
            var season = _mapper.Map<Season>(seasonDto);
            await _unitOfWork
                .GetReadWriteRepository<Season>()
                .SaveAsync(season);
            
            if (seasonDto.VideosDto.Count > 0)
            {
                seasonDto.VideosDto
                    .ForEach(v => 
                        v.SeasonId = season.Id);
                var result = await AddVideoAsync(seasonDto.VideosDto);
                if (!result.IsSuccessful)
                {
                    await _unitOfWork
                        .GetReadWriteRepository<Season>()
                        .RemoveAsync(season);
                    return result;
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
            
            return new HttpResult();
        }
    }
}