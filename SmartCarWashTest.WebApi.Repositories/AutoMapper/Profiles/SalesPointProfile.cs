using System.Linq;
using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTOs.Models.ProvidedProduct;
using SmartCarWashTest.WebApi.DTOs.Models.SalesPoint;

namespace SmartCarWashTest.WebApi.Repositories.AutoMapper.Profiles
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
                        .ToList()))
                .ConstructUsing(_ => new SalesPointReadModel(default, default, default));
            CreateMap<SalesPointReadModel, SalesPoint>().ConstructUsing(_ => new SalesPoint(default, default));
            CreateMap<SalesPointCreateModel, SalesPoint>().ConstructUsing(_ => new SalesPoint(default, default));
            CreateMap<SalesPointUpdateModel, SalesPoint>().ConstructUsing(_ => new SalesPoint(default, default));
            CreateMap<SalesPointCreateModel, SalesPointReadModel>()
                .ConstructUsing(_ => new SalesPointReadModel(default, default, default));
            CreateMap<SalesPointUpdateModel, SalesPointReadModel>()
                .ConstructUsing(_ => new SalesPointReadModel(default, default, default));
        }
    }
}