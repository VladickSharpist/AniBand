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

namespace AniBand.Video.Services.Services
{
    public class VideoService
        : IVideoService
    {
        private readonly IBaseReadonlyRepository<Season> _seasonReadRepository;
        private readonly IBaseReadWriteRepository<Season> _seasonWriteRepository;
        private readonly IBaseReadonlyRepository<Studio> _studioWriteRepository;
        private readonly IBaseReadWriteRepository<Domain.Models.Video> _videoWriteRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public VideoService(
            IBaseReadonlyRepository<Season> seasonRepository,
            IBaseReadWriteRepository<Domain.Models.Video> videoWriteRepository,
            IMapper mapper,
            IFileService fileService, 
            IBaseReadWriteRepository<Season> seasonWriteRepository,
            IBaseReadonlyRepository<Studio> studioWriteRepository)
        {
            _seasonReadRepository = seasonRepository
                ?? throw new NullReferenceException(nameof(seasonRepository));
            _videoWriteRepository = videoWriteRepository
                ?? throw new NullReferenceException(nameof(videoWriteRepository));
            _mapper = mapper
                ?? throw new NullReferenceException(nameof(mapper));
            _fileService = fileService
                ?? throw new NullReferenceException(nameof(fileService));
            _seasonWriteRepository = seasonWriteRepository
                ?? throw new NullReferenceException(nameof(seasonWriteRepository));
            _studioWriteRepository = studioWriteRepository 
                ?? throw new NullReferenceException(nameof(studioWriteRepository));
        }

        public async Task<IHttpResult> AddVideoAsync(List<VideoDto> videos)
        {
            if (videos == null)
            {
                return new HttpResult(
                    "Files count doesnt match Videos count", 
                    HttpStatusCode.UnprocessableEntity);
            }
            foreach (var videoDto in videos)
            {
                var season = (await _seasonReadRepository
                        .GetNoTrackingAsync(v =>
                            v.Id == videoDto.SeasonId))
                    .SingleOrDefault();

                if (season == null)
                {
                    return new HttpResult(
                        "No such season", 
                        HttpStatusCode.BadRequest);
                }

                var video = _mapper.Map<Domain.Models.Video>(videoDto);
                await _videoWriteRepository.SaveAsync(video);
                
                var fileName = season.Id + "-" + video.Id;
                try
                {
                    video.Url = _fileService
                        .StoreFileGetUrl(videoDto.VideoFile, fileName);
                    video.DurationInSeconds = _fileService
                        .GetVideoDuration(video.Url);
                }
                catch (Exception e)
                {
                    await _videoWriteRepository.RemoveAsync(video);
                    return new HttpResult(
                        e.Message, 
                        HttpStatusCode.BadRequest);
                }
                
                await _videoWriteRepository.SaveAsync(video);
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
            
            var studio = (await _studioWriteRepository
                    .GetNoTrackingAsync(s =>
                        s.Id == seasonDto.StudioId))
                .SingleOrDefault();
            
            var season = _mapper.Map<Season>(seasonDto);
            await _seasonWriteRepository.SaveAsync(season);
            
            if (seasonDto.VideosDto.Count > 0)
            {
                seasonDto.VideosDto
                    .ForEach(v => 
                        v.SeasonId = season.Id);
                var result = await AddVideoAsync(seasonDto.VideosDto);
                if (!result.IsSuccessful)
                {
                    await _seasonWriteRepository.RemoveAsync(season);
                    return result;
                }
            }
            
            var imageFileName = studio != null 
                ? studio.Id + "-" + season.Id
                : "NoStudio-" + season.Id;
            
            season.ImageUrl = _fileService
                .StoreFileGetUrl(seasonDto.Image, imageFileName);
            
            
            await _seasonWriteRepository.SaveAsync(season);
            
            return new HttpResult();
        }
    }
}