using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite.Contexts;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.CRUD.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Repositories
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