using SmartCarWashTest.WebApi.DTOs.Models.SalesData;

namespace SmartCarWashTest.WebApi.Repositories.Interfaces
{
    public interface ISalesDataCacheRepository :
        IRepository<SalesDataCreateModel, SalesDataReadModel, SalesDataUpdateModel>
    {
    }
}