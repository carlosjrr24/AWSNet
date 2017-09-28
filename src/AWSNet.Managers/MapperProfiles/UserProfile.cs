using AutoMapper;
using AWSNet.Dtos;
using AWSNet.Model;

namespace AWSNet.Managers.MapperProfiles
{
    internal class UserProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<User, UserDto>().ForMember(x => x.Roles, opt => opt.Ignore());

            CreateMap<UserDto, User>()
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(x => x.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(x => x.State, opt => opt.MapFrom(src => src.State))
                .ForMember(x => x.City, opt => opt.MapFrom(src => src.City))
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(x => x.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(x => x.CellPhone, opt => opt.MapFrom(src => src.CellPhone))
                .ForMember(x => x.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(x => x.TimeZoneId, opt => opt.MapFrom(src => src.TimeZoneId))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
