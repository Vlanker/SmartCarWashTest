using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTO.Models.Product;

namespace SmartCarWashTest.WebApi.Repositories.AutoMapper.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductReadModel>().ConstructUsing(_ => new ProductReadModel(default, default, default));
            CreateMap<ProductReadModel, Product>().ConstructUsing(_ => new Product(default, default, default));
            CreateMap<ProductCreateModel, Product>().ConstructUsing(_ => new Product(default, default, default));
            CreateMap<ProductUpdateModel, Product>().ConstructUsing(_ => new Product(default, default, default));
            CreateMap<ProductCreateModel, ProductReadModel>()
                .ConstructUsing(_ => new ProductReadModel(default, default, default));
            CreateMap<ProductUpdateModel, ProductReadModel>()
                .ConstructUsing(_ => new ProductReadModel(default, default, default));
        }
    }
}