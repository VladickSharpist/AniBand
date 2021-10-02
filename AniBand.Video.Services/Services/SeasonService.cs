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
    public class SeasonService
        : ISeasonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IVideoService _videoService;

        public SeasonService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IFileService fileService, 
            IVideoService videoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _videoService = videoService;
        }

        public async Task<IHttpResult> AddSeasonAsync(SeasonDto seasonDto)
        {
            if (seasonDto.Videos == null)
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
                    var season = _mapper.Map<Anime>(seasonDto);
                    await _unitOfWork
                        .GetReadWriteRepository<Anime>()
                        .SaveAsync(season);

                    if (seasonDto.Videos.ToList().Count > 0)
                    {
                        seasonDto.Videos.ToList()
                            .ForEach(v =>
                                v.SeasonId = season.Id);
                        foreach (var videoDto in seasonDto.Videos)
                        {
                            var result = await _videoService.SaveVideoAsync(videoDto);
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
                        .GetReadWriteRepository<Anime>()
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

        public async Task<IHttpResult> DeleteSeasonByIdAsync(long id)
        {
            var seasonRepo = _unitOfWork
                .GetReadWriteRepository<Anime>();

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
    }
}