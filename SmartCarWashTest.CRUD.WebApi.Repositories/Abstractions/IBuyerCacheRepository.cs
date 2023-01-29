using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions
{
    public interface IBuyerCacheRepository : IRepository<BuyerCreateModel, BuyerReadModel, BuyerUpdateModel>
    {
    }
}