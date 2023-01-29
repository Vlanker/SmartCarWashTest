using System;
using System.Collections.Generic;
using SmartCarWashTest.EventBus.Events;

namespace SmartCarWashTest.Sale.WebApi.DTOs.Events
{
    /// <summary>
    /// Sale published IntegrationEvent.
    /// </summary>
    /// <param name="Date"> Date. </param>
    /// <param name="Time"> Time. </param>
    /// <param name="SalesPointId"> Sales point ID. </param>
    /// <param name="BuyerId"> Buyer ID. </param>
    /// <param name="TotalAmount"> Total amount. </param>
    public record SalePublishedIntegrationEvent(DateTime Date, TimeSpan Time, int SalesPointId, int? BuyerId,
        string BuyerName, decimal TotalAmount) : IntegrationEvent
    {
        public SalePublishedIntegrationEvent() : this(DateTime.MinValue.Date, TimeSpan.Zero, default, default,
            string.Empty, default)
        {
            SalesData = new List<InternalSalesDataPublishedIntegrationEvent>();
        }

        public IEnumerable<InternalSalesDataPublishedIntegrationEvent> SalesData { get; set; }
    }
}