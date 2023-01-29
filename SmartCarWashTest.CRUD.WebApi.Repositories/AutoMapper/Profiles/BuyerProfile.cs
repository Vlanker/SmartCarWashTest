using System.Linq;
using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.AutoMapper.Profiles
{
    public class BuyerProfile : Profile
    {
        public BuyerProfile()
        {
            CreateMap<Buyer, BuyerReadModel>()
                .ForMember(dest => dest.SalesIds,
                    act => act.MapFrom(buyer => buyer.Sales.Select(sale => sale.Id).ToList()));
            CreateMap<BuyerReadModel, Buyer>();
            CreateMap<BuyerCreateModel, Buyer>();
            CreateMap<BuyerUpdateModel, Buyer>();
            CreateMap<BuyerCreateModel, BuyerReadModel>();
            CreateMap<BuyerUpdateModel, BuyerReadModel>();
        }
    }
}