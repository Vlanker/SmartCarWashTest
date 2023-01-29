using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SmartCarWashTest.Common.Converters.JsonConverters;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.Sale
{
    /// <summary>
    /// Sale create model.
    /// </summary>
    /// <param name="SalesPointId">Point of sale ID in Sale model.</param>
    /// <param name="BuyerId">Buyer ID in Sale model.</param>
    /// <param name="TotalAmount">Total amount.</param>
    public record SaleCreateModel([property: Required] int SalesPointId, [property: Required] int? BuyerId,
        [property: Required] decimal TotalAmount) : ICreateModel
    {
        public SaleCreateModel() : this(default, default, default)
        {
            Date = DateTime.MinValue.Date;
            Time = TimeSpan.Zero;
        }

        /// <summary>
        /// Date of sale.
        /// </summary>
        [property: Required]
        [property: JsonConverter(typeof(DateTimeToDateConverter))]
        public DateTime Date { get; init; }

        /// <summary>
        /// Time of sale.
        /// </summary>
        [property: Required]
        [property: JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Time { get; init; }
    }
}