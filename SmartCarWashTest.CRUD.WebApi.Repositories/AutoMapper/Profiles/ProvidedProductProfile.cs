using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.AutoMapper.Profiles
{
    public class ProvidedProductProfile : Profile
    {
        public ProvidedProductProfile()
        {
            CreateMap<ProvidedProduct, ProvidedProductReadModel>();
            CreateMap<ProvidedProductReadModel, ProvidedProduct>();
            CreateMap<ProvidedProductCreateModel, ProvidedProduct>();
            CreateMap<ProvidedProductUpdateModel, ProvidedProduct>();
            CreateMap<ProvidedProductCreateModel, ProvidedProductReadModel>();
            CreateMap<ProvidedProductUpdateModel, ProvidedProductReadModel>();
            CreateMap<ProvidedProductReadModel, ProvidedProductUpdateModel>();
        }
    }
}