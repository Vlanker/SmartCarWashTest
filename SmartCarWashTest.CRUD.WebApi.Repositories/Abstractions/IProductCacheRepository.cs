using SmartCarWashTest.CRUD.WebApi.DTOs.Models.Product;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions
{
    public interface IProductCacheRepository : IRepository<ProductCreateModel, ProductReadModel, ProductUpdateModel>
    {
    }
}