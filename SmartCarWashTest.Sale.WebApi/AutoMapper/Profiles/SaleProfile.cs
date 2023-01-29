using System.Linq;
using AutoMapper;
using SmartCarWashTest.Sale.WebApi.DTOs.Events;

namespace SmartCarWashTest.Sale.WebApi.AutoMapper.Profiles
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<DTOs.Models.Sale, SalePublishedIntegrationEvent>()
                .ForMember(dest => dest.SalesData,
                    act => act.MapFrom(model => model.SalesData
                        .Select(data =>
                            new InternalSalesDataPublishedIntegrationEvent(data.ProductId, data.ProductQuantity,
                                data.ProductIdAmount))));

        }
    }
}