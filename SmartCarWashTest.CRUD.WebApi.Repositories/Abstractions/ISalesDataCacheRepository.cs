using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions
{
    public interface ISalesDataCacheRepository :
        IRepository<SalesDataCreateModel, SalesDataReadModel, SalesDataUpdateModel>
    {
    }
}