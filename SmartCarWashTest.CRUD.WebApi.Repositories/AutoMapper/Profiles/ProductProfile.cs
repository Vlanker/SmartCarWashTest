using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Product;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.AutoMapper.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductReadModel>();
            CreateMap<ProductReadModel, Product>();
            CreateMap<ProductCreateModel, Product>();
            CreateMap<ProductUpdateModel, Product>();
            CreateMap<ProductCreateModel, ProductReadModel>();
            CreateMap<ProductUpdateModel, ProductReadModel>();
        }
    }
}