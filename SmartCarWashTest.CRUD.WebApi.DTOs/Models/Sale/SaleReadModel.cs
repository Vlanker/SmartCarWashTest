using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using SmartCarWashTest.Common.Converters.JsonConverters;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData;
using TimeSpanConverter = SmartCarWashTest.Common.Converters.JsonConverters.TimeSpanConverter;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.Sale
{
    /// <summary>
    /// Sale read model.
    /// </summary>
    /// <param name="Id">Sale ID.</param>
    /// <param name="SalesPointId">Point of sale ID in Sale model.</param>
    /// <param name="BuyerId">Buyer ID in Sale model.</param>
    /// <param name="TotalAmount">Total amount.</param>
    /// <param name="SalesData">List of Sale data model.</param>
    public record SaleReadModel(int Id, int SalesPointId, int? BuyerId, decimal TotalAmount,
        [property: XmlIgnore] IEnumerable<SalesDataReadModel> SalesData) : IReadModel, IHaveIdentifier
    {
        public SaleReadModel() : this(default, default, default, default, default)
        {
            Date = DateTime.MinValue.Date;
            Time = TimeSpan.Zero;
        }

        /// <summary>
        /// Date of sale.
        /// </summary>
        [property: JsonConverter(typeof(DateTimeToDateConverter))]
        public DateTime Date { get; init; }

        /// <summary>
        /// Time of sale.
        /// </summary>
        [property: JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Time { get; init; }
    }
}