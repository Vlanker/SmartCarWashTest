using SmartCarWashTest.WebApi.DTO.Models.ProvidedProduct;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface IProvidedProductCacheRepository :
        IRepository<ProvidedProductCreateModel, ProvidedProductReadModel, ProvidedProductUpdateModel>
    {
    }
}