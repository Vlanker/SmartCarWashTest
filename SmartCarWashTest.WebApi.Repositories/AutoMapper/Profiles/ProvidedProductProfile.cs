using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTO.Models.ProvidedProduct;

namespace SmartCarWashTest.WebApi.Repositories.AutoMapper.Profiles
{
    public class ProvidedProductProfile : Profile
    {
        public ProvidedProductProfile()
        {
            CreateMap<ProvidedProduct, ProvidedProductReadModel>().ConstructUsing(_ =>
                new ProvidedProductReadModel(default, default, default, default));
            CreateMap<ProvidedProductReadModel, ProvidedProduct>()
                .ConstructUsing(_ => new ProvidedProduct(default, default, default, default));
            CreateMap<ProvidedProductCreateModel, ProvidedProduct>()
                .ConstructUsing(_ => new ProvidedProduct(default, default, default, default));
            CreateMap<ProvidedProductUpdateModel, ProvidedProduct>()
                .ConstructUsing(_ => new ProvidedProduct(default, default, default, default));
            CreateMap<ProvidedProductCreateModel, ProvidedProductReadModel>().ConstructUsing(_ =>
                new ProvidedProductReadModel(default, default, default, default));
            CreateMap<ProvidedProductUpdateModel, ProvidedProductReadModel>().ConstructUsing(_ =>
                new ProvidedProductReadModel(default, default, default, default));
        }
    }
}