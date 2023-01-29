using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.AutoMapper.Profiles
{
    public class SalesDataProfile : Profile
    {
        public SalesDataProfile()
        {
            CreateMap<SalesData, SalesDataReadModel>();
            CreateMap<SalesDataReadModel, SalesData>();
            CreateMap<SalesDataCreateModel, SalesData>();
            CreateMap<SalesDataUpdateModel, SalesData>();
            CreateMap<SalesDataCreateModel, SalesDataReadModel>();
            CreateMap<SalesDataUpdateModel, SalesDataReadModel>();
        }
    }
}