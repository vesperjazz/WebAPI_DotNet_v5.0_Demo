using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Weather;
using WebAPI_DotNetCore_Demo.Application.MappingProfiles;
using WebAPI_DotNetCore_Demo.Infrastructure.Models;

namespace WebAPI_DotNetCore_Demo.Infrastructure.MappingProfiles
{
    public class WeatherProfileMapping : ProfileBase
    {
        protected override void MapDomainToDataTransferObject()
        {
            CreateMap<RainfallModel, RainfallDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ApiInfo.Status))
                .ForMember(dest => dest.ReadingType, opt => opt.MapFrom(src => src.Metadata.ReadingType))
                .ForMember(dest => dest.ReadingUnit, opt => opt.MapFrom(src => src.Metadata.ReadingUnit))
                .ForMember(dest => dest.Stations, opt => opt.MapFrom(src => src.Metadata.Stations));

            CreateMap<StationModel, StationDto>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Longitude));

            CreateMap<ItemModel, ItemDto>();
            CreateMap<ReadingModel, ReadingDto>();

            CreateMap<RelativeHumidityModel, RelativeHumidityDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ApiInfo.Status))
                .ForMember(dest => dest.ReadingType, opt => opt.MapFrom(src => src.Metadata.ReadingType))
                .ForMember(dest => dest.ReadingUnit, opt => opt.MapFrom(src => src.Metadata.ReadingUnit))
                .ForMember(dest => dest.Stations, opt => opt.MapFrom(src => src.Metadata.Stations));
        }

        protected override void MapDataTransferObjectToDomain()
        {
        }
    }
}
