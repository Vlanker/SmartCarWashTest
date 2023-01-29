using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite.Contexts;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.CRUD.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Repositories
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