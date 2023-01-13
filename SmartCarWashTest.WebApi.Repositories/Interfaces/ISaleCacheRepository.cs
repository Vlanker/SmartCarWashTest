using SmartCarWashTest.WebApi.DTOs.Models.Sale;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface ISaleCacheRepository : IRepository<SaleCreateModel, SaleReadModel, SaleUpdateModel>
    {
    }
}