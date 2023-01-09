using SmartCarWashTest.WebApi.DTO.Models.Sale;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface ISaleCacheRepository : IRepository<SaleCreateModel, SaleReadModel, SaleUpdateModel>
    {
    }
}