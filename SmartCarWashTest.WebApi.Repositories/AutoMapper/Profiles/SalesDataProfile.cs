using AutoMapper;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTOs.Models.SalesData;

namespace SmartCarWashTest.WebApi.Repositories.AutoMapper.Profiles
{
    public class SalesDataProfile : Profile
    {
        public SalesDataProfile()
        {
            CreateMap<SalesData, SalesDataReadModel>().ConstructUsing(_ =>
                new SalesDataReadModel(default, default, default, default, default));
            CreateMap<SalesDataReadModel, SalesData>()
                .ConstructUsing(_ => new SalesData(default, default, default, default, default));
            CreateMap<SalesDataCreateModel, SalesData>()
                .ConstructUsing(_ => new SalesData(default, default, default, default, default));
            CreateMap<SalesDataUpdateModel, SalesData>()
                .ConstructUsing(_ => new SalesData(default, default, default, default, default));
            CreateMap<SalesDataCreateModel, SalesDataReadModel>().ConstructUsing(_ =>
                new SalesDataReadModel(default, default, default, default, default));
            CreateMap<SalesDataUpdateModel, SalesDataReadModel>().ConstructUsing(_ =>
                new SalesDataReadModel(default, default, default, default, default));
        }
    }
}