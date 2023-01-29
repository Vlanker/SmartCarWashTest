using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using SmartCarWashTest.Common.Converters.JsonConverters;

namespace SmartCarWashTest.Sale.WebApi.DTOs.Models
{
    /// <summary>
    /// Sale.
    /// </summary>
    /// <param name="SalePointId">SalePoint ID.</param>
    /// <param name="BuyerId">Buyer ID.</param>
    /// <param name="BuyerName">Buyer name.</param>
    /// <param name="TotalAmount">Total amount.</param>
    /// <param name="SalesData">List SalesData.</param>
    public record Sale([property: Required] int SalePointId, int? BuyerId, string BuyerName,
        [property: Required] decimal TotalAmount, [property: XmlIgnore] IEnumerable<SalesData> SalesData)
    {
        public Sale() : this(0, null, string.Empty, 0, new[] { new SalesData() })
        {
            Date = DateTime.MinValue.Date;
            Time = TimeSpan.Zero;
        }

        /// <summary>
        /// Date.
        /// </summary>
        [property: Required]
        [property: JsonConverter(typeof(DateTimeToDateConverter))]
        public DateTime Date { get; init; }

        /// <summary>
        /// Time.
        /// </summary>
        [property: Required]
        [property: JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Time { get; init; }
    }
}