using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using SmartCarWashTest.WebApi.DTO.Converters;
using SmartCarWashTest.WebApi.DTO.Interfaces;
using SmartCarWashTest.WebApi.DTO.Models.SalesData;
using TimeSpanConverter = SmartCarWashTest.WebApi.DTO.Converters.TimeSpanConverter;

namespace SmartCarWashTest.WebApi.DTO.Models.Sale
{
    /// <summary>
    /// Sale read model.
    /// </summary>
    /// <param name="Id">Sale ID.</param>
    /// <param name="SalesPointId">Point of sale ID in Sale model.</param>
    /// <param name="BuyerId">Buyer ID in Sale model.</param>
    /// <param name="TotalAmount">Total amount.</param>
    /// <param name="SalesData">List of Sale data model.</param>
    public record SaleReadModel(int Id, int SalesPointId, int BuyerId, decimal TotalAmount,
        [XmlIgnore] IEnumerable<SalesDataReadModel> SalesData) : IReadModel, IHaveIdentifier
    {
        /// <summary>
        /// Date of sale.
        /// </summary>
        [JsonConverter(typeof(DateTimeToDateConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Time of sale.
        /// </summary>
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Time { get; set; }
    }
}