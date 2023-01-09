using System.Linq;
using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTO.Models.Buyer;

namespace SmartCarWashTest.WebApi.Repositories.AutoMapper.Profiles
{
    public class BuyerProfile : Profile
    {
        public BuyerProfile()
        {
            CreateMap<Buyer, BuyerReadModel>()
                .ForMember(dest => dest.SalesIds,
                    act => act.MapFrom(buyer => buyer.Sales.Select(sale => sale.Id).ToList()))
                .ConstructUsing(_ => new BuyerReadModel(default, default, default));
            CreateMap<BuyerReadModel, Buyer>().ConstructUsing(_ => new Buyer(default, default));
            CreateMap<BuyerCreateModel, Buyer>().ConstructUsing(_ => new Buyer(default, default));
            CreateMap<BuyerUpdateModel, Buyer>().ConstructUsing(_ => new Buyer(default, default));
            CreateMap<BuyerCreateModel, BuyerReadModel>()
                .ConstructUsing(_ => new BuyerReadModel(default, default, default));
            CreateMap<BuyerUpdateModel, BuyerReadModel>()
                .ConstructUsing(_ => new BuyerReadModel(default, default, default));
        }
    }
}