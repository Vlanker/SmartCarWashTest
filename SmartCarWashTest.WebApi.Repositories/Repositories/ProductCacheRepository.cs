using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTOs.Models.Product;
using SmartCarWashTest.WebApi.Repositories.Interfaces;
using SmartCarWashTest.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.WebApi.Repositories.Repositories
{
    public class ProductCacheRepository : 
        BaseCacheRepository<ProductCreateModel, ProductReadModel, ProductUpdateModel, Product>,
        IProductCacheRepository
    {
        public ProductCacheRepository(SmartCarWashContext smartCarWashContext, IMapper mapper) :
            base(smartCarWashContext, mapper)
        {
        }
    }
}