using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite.Contexts;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.CRUD.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Repositories
{
    public class SalesDataCacheRepository :
        BaseCacheRepository<SalesDataCreateModel, SalesDataReadModel, SalesDataUpdateModel, SalesData>,
        ISalesDataCacheRepository
    {
        public SalesDataCacheRepository(SmartCarWashContext smartCarWashContext, IMapper mapper) :
            base(smartCarWashContext, mapper)
        {
        }
    }
}