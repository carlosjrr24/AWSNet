using AutoMapper;
using AWSNet.Dtos;
using AWSNet.Model;

namespace AWSNet.Managers.MapperProfiles
{
    //internal class CategoryProfile : Profile
    //{
    //    protected override void Configure()
    //    {
    //        CreateMap<Category, CategoryDto>()
    //            .ForMember(x => x.Media, opt => opt.Ignore());

    //        CreateMap<Category, DeleteCategoryDtoBindingModel>()
    //            .ForMember(x => x.Media, opt => opt.Ignore());

    //        CreateMap<CategoryDto, Category>()
    //            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
    //            .ForMember(x => x.IsEnabled, opt => opt.MapFrom(src => src.IsEnabled))
    //            .ForAllOtherMembers(x => x.Ignore());
    //    }
    //}

    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
