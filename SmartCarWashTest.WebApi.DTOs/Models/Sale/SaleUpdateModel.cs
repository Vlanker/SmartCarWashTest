using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SmartCarWashTest.Common.Converters.JsonConverters;
using SmartCarWashTest.WebApi.DTOs.Interfaces;
using TimeSpanConverter = SmartCarWashTest.Common.Converters.JsonConverters.TimeSpanConverter;

namespace SmartCarWashTest.WebApi.DTOs.Models.Sale
{
    /// <summary>
    /// Sale update model.
    /// </summary>
    /// <param name="Id">Sale ID.</param>
    /// <param name="SalesPointId">Point of sale ID in Sale model.</param>
    /// <param name="BuyerId">Buyer ID in Sale model.</param>
    /// <param name="TotalAmount">Total amount.</param>
    public record SaleUpdateModel([Required] int Id, int SalesPointId, int BuyerId, [Required] decimal TotalAmount) : 
        IUpdateModel
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