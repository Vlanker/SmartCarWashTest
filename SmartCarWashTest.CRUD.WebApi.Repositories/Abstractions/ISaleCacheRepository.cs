using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Sale;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions
{
    public interface ISaleCacheRepository : IRepository<SaleCreateModel, SaleReadModel, SaleUpdateModel>
    {
    }
}