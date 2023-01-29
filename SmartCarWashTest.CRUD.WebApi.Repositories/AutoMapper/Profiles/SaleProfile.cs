using System.Linq;
using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Sale;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.AutoMapper.Profiles
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
                        .ToList()));
            CreateMap<SaleReadModel, Sale>();
            CreateMap<SaleCreateModel, Sale>();
            CreateMap<SaleUpdateModel, Sale>();
            CreateMap<SaleCreateModel, SaleReadModel>();
            CreateMap<SaleUpdateModel, SaleReadModel>();
        }
    }
}