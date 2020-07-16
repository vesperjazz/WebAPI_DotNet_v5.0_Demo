using WebAPI_DotNetCore_Demo.Application.DataTransferObjects;
using WebAPI_DotNetCore_Demo.Domain.Entities;

namespace WebAPI_DotNetCore_Demo.Application.MappingProfiles
{
    public class PersonProfileMapping : ProfileBase
    {
        protected override void MapDomainToDataTransferObject()
        {
            CreateMap<PhoneNumber, PhoneNumberDto>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country.CountryCode));
            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.CountryISOCode, opt => opt.MapFrom(src => src.Country.ISOCode));
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender.Name));
        }

        protected override void MapDataTransferObjectToDomain()
        {
            CreateMap<CreatePersonDto, Person>();
            CreateMap<CreatePhoneNumberDto, PhoneNumber>();
            CreateMap<CreateAddressDto, Address>();

            CreateMap<UpdatePersonDto, Person>();
            CreateMap<UpdatePhoneNumberDto, PhoneNumber>();
            CreateMap<UpdateAddressDto, Address>();

            CreateMap<UpdatePersonNameDto, Person>();
        }
    }
}
