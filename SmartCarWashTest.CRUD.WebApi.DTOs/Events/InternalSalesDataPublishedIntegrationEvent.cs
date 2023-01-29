using SmartCarWashTest.EventBus.Events;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Events
{
    
    /// <summary>
    /// Sale data published IntegrationEvent.
    /// </summary>
    /// <param name="ProductId"> Product ID. </param>
    /// <param name="ProductQuantity"> Production quantity. </param>
    /// <param name="ProductIdAmount"> ProductionId Amount. </param>
    public record InternalSalesDataPublishedIntegrationEvent( int ProductId, int ProductQuantity,
        int ProductIdAmount) : IntegrationEvent;
}