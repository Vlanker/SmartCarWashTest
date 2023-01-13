using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTOs.Models.SalesPoint;
using SmartCarWashTest.WebApi.Repositories.Interfaces;
using SmartCarWashTest.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.WebApi.Repositories.Repositories
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