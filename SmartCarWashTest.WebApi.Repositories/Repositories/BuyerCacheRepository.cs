using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTO.Models.Buyer;
using SmartCarWashTest.WebApi.Repositories.Interfaces;
using SmartCarWashTest.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.WebApi.Repositories.Repositories
{
    public class BuyerCacheRepository :
        BaseCacheRepository<BuyerCreateModel, BuyerReadModel, BuyerUpdateModel, Buyer>,
        IBuyerCacheRepository
    {
        public BuyerCacheRepository(SmartCarWashContext smartCarWashContext, IMapper mapper) :
            base(smartCarWashContext, mapper)
        {
        }
    }
}