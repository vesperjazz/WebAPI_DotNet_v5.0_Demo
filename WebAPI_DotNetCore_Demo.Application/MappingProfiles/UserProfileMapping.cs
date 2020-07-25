using System.Linq;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users;
using WebAPI_DotNetCore_Demo.Domain.Entities.Users;

namespace WebAPI_DotNetCore_Demo.Application.MappingProfiles
{
    public class UserProfileMapping : ProfileBase
    {
        protected override void MapDomainToDataTransferObject()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));
        }

        protected override void MapDataTransferObjectToDomain()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.RoleIDs.Select(rid => new UserRole { RoleID = rid })));
        }
    }
}
