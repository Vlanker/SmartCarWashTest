using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesPoint;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions
{
    public interface ISalesPointCacheRepository :
        IRepository<SalesPointCreateModel, SalesPointReadModel, SalesPointUpdateModel>
    {
    }
}