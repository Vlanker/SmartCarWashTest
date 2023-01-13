using SmartCarWashTest.WebApi.DTOs.Models.ProvidedProduct;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface IProvidedProductCacheRepository :
        IRepository<ProvidedProductCreateModel, ProvidedProductReadModel, ProvidedProductUpdateModel>
    {
    }
}