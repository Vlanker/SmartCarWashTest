using System;
using System.Collections.Generic;
using SmartCarWashTest.EventBus.Events;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Events
{
    /// <summary>
    /// Sale published IntegrationEvent.
    /// </summary>
    /// <param name="Date"> Date. </param>
    /// <param name="Time"> Time. </param>
    /// <param name="SalesPointId"> Sales point ID. </param>
    /// <param name="BuyerId"> Buyer ID. </param>
    /// <param name="TotalAmount"> Total amount. </param>
    /// <param name="SalesDataPublishedIntegrationEvent"> List of SalesData published IntegrationEvent. </param>
    public record SalePublishedIntegrationEvent(DateTime Date,
        TimeSpan Time,
        int SalesPointId,
        int? BuyerId,
        string BuyerName,
        decimal TotalAmount,
        List<InternalSalesDataPublishedIntegrationEvent> SalesDataPublishedIntegrationEvent) : IntegrationEvent;
}