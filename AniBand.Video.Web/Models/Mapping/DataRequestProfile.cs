using AniBand.Query.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class DataRequestProfile
        : Profile
    {
        public DataRequestProfile()
        {
            CreateMap<DataRequestVm, QueryDto>()
                .ForAllMembers(opt => 
                    opt.AllowNull());
        }
    }
}