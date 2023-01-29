using SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions
{
    public interface IProvidedProductCacheRepository :
        IRepository<ProvidedProductCreateModel, ProvidedProductReadModel, ProvidedProductUpdateModel>
    {
    }
}