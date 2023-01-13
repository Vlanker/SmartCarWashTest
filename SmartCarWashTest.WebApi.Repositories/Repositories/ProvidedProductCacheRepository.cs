using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTOs.Models.ProvidedProduct;
using SmartCarWashTest.WebApi.Repositories.Interfaces;
using SmartCarWashTest.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.WebApi.Repositories.Repositories
{
    public class ProvidedProductCacheRepository :
        BaseCacheRepository<ProvidedProductCreateModel, ProvidedProductReadModel, ProvidedProductUpdateModel,
            ProvidedProduct>,
        IProvidedProductCacheRepository
    {
        public ProvidedProductCacheRepository(SmartCarWashContext smartCarWashContext, IMapper mapper) :
            base(smartCarWashContext, mapper)
        {
        }
    }
}