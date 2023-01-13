using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.WebApi.DTOs.Models.Sale;
using SmartCarWashTest.WebApi.Repositories.Interfaces;
using SmartCarWashTest.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.WebApi.Repositories.Repositories
{
    public class SaleCacheRepository : 
        BaseCacheRepository<SaleCreateModel, SaleReadModel, SaleUpdateModel, Sale>,
        ISaleCacheRepository
    {
        public SaleCacheRepository(SmartCarWashContext smartCarWashContext, IMapper mapper) :
            base(smartCarWashContext, mapper)
        {
        }
    }
}