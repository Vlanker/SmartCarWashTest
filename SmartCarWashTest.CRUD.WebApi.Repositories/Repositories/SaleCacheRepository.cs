using AutoMapper;
using SmartCarWashTest.Common.DataContext.Sqlite.Contexts;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Sale;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.CRUD.WebApi.Repositories.Repositories.Base;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Repositories
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