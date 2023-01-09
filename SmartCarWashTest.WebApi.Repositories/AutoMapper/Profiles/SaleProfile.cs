using System.Linq;
using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTO.Models.Sale;
using SmartCarWashTest.WebApi.DTO.Models.SalesData;

namespace SmartCarWashTest.WebApi.Repositories.AutoMapper.Profiles
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, SaleReadModel>()
                .ForMember(dest => dest.SalesData,
                    act => act.MapFrom(model => model.SalesDataSet
                        .Select(data => new SalesDataReadModel(data.Id,
                            data.SaleId,
                            data.ProductId,
                            data.ProductQuantity,
                            data.ProductIdAmount))
                        .ToList()))
                .ConstructUsing(_ => new SaleReadModel(default, default, default, default, default));
            CreateMap<SaleReadModel, Sale>()
                .ConstructUsing(_ => new Sale(default, default, default, default, default, default));
            CreateMap<SaleCreateModel, Sale>()
                .ConstructUsing(_ => new Sale(default, default, default, default, default, default));
            CreateMap<SaleUpdateModel, Sale>()
                .ConstructUsing(_ => new Sale(default, default, default, default, default, default));
            CreateMap<SaleCreateModel, SaleReadModel>()
                .ConstructUsing(_ => new SaleReadModel(default, default, default, default, default));
            CreateMap<SaleUpdateModel, SaleReadModel>()
                .ConstructUsing(_ => new SaleReadModel(default, default, default, default, default));
        }
    }
}