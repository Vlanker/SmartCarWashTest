using SmartCarWashTest.WebApi.DTOs.Models.Product;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface IProductCacheRepository : IRepository<ProductCreateModel, ProductReadModel, ProductUpdateModel>
    {
    }
}