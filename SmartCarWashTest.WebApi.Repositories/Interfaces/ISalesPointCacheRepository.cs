using SmartCarWashTest.WebApi.DTOs.Models.SalesPoint;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface ISalesPointCacheRepository :
        IRepository<SalesPointCreateModel, SalesPointReadModel, SalesPointUpdateModel>
    {
    }
}