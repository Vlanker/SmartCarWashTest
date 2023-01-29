using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SmartCarWashTest.Common.Converters.JsonConverters;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;
using TimeSpanConverter = SmartCarWashTest.Common.Converters.JsonConverters.TimeSpanConverter;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.Sale
{
    /// <summary>
    /// Sale update model.
    /// </summary>
    /// <param name="Id">Sale ID.</param>
    /// <param name="SalesPointId">Point of sale ID in Sale model.</param>
    /// <param name="BuyerId">Buyer ID in Sale model.</param>
    /// <param name="TotalAmount">Total amount.</param>
    public record SaleUpdateModel([property: Required] int Id, int SalesPointId, int? BuyerId,
        [property: Required] decimal TotalAmount) : IUpdateModel
    {
        public SaleUpdateModel() : this(default, default, default, default)
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