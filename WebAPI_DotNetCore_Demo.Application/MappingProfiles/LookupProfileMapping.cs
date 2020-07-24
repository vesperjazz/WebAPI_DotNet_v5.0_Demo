using AutoMapper;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Lookups;
using WebAPI_DotNetCore_Demo.Domain.Entities.Lookups;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Application.MappingProfiles
{
    public class LookupProfileMapping : ProfileBase
    {
        protected override void MapDomainToDataTransferObject()
        {
            CreateMap<Gender, GenderDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Role, RoleDto>();
        }

        protected override void MapDataTransferObjectToDomain()
        {
        }
    }
}
