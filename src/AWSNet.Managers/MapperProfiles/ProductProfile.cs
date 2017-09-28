using AutoMapper;
using AWSNet.Dtos;
using AWSNet.Model;

namespace AWSNet.Managers.MapperProfiles
{
    internal class ProductProfile : Profile
    {
        //protected override void Configure()
        //{

        //    CreateMap<Product, ProductDto>()
        //        .ForMember(x => x.Media, opt => opt.Ignore());

        //    CreateMap<Product, DeleteProductDtoBindingModel>()
        //        .ForMember(x => x.Media, opt => opt.Ignore());



        //}

        public ProductProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.IsEnabled, opt => opt.MapFrom(src => src.IsEnabled))
                .ForAllOtherMembers(x => x.Ignore());

        }

    }
}
