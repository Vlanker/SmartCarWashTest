using SmartCarWashTest.WebApi.DTOs.Models.Buyer;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface IBuyerCacheRepository : IRepository<BuyerCreateModel, BuyerReadModel, BuyerUpdateModel>
    {
    }
}