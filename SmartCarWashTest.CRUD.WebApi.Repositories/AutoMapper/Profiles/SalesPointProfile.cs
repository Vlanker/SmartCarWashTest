using System.Linq;
using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesPoint;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.AutoMapper.Profiles
{
    public class SalesPointProfile : Profile
    {
        public SalesPointProfile()
        {
            CreateMap<SalesPoint, SalesPointReadModel>()
                .ForMember(dest => dest.ProvidedProducts,
                    act => act.MapFrom(model => model.ProvidedProduct
                        .Select(data => new ProvidedProductReadModel(data.Id,
                            data.ProductId,
                            data.ProductQuantity,
                            data.SalesPointId))
                        .ToList()));
            CreateMap<SalesPointReadModel, SalesPoint>();
            CreateMap<SalesPointCreateModel, SalesPoint>();
            CreateMap<SalesPointUpdateModel, SalesPoint>();
            CreateMap<SalesPointCreateModel, SalesPointReadModel>();
            CreateMap<SalesPointUpdateModel, SalesPointReadModel>();
        }
    }
}