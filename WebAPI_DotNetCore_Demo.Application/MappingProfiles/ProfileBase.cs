using AutoMapper;

namespace WebAPI_DotNetCore_Demo.Application.MappingProfiles
{
    public abstract class ProfileBase : Profile
    {
        protected ProfileBase()
        {
            MapDomainToDataTransferObject();

            MapDataTransferObjectToDomain();
        }

        protected abstract void MapDomainToDataTransferObject();
        protected abstract void MapDataTransferObjectToDomain();
    }
}
