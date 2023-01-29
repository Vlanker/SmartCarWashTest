using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite.Contexts;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesPoint;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.CRUD.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Repositories
{
    public class SalesPointCacheRepository :
        BaseCacheRepository<SalesPointCreateModel, SalesPointReadModel, SalesPointUpdateModel, SalesPoint>,
        ISalesPointCacheRepository
    {
        public SalesPointCacheRepository(SmartCarWashContext smartCarWashContext, IMapper mapper) :
            base(smartCarWashContext, mapper)
        {
        }
    }
}