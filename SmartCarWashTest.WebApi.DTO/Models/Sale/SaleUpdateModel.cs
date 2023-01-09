using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SmartCarWashTest.WebApi.DTO.Converters;
using SmartCarWashTest.WebApi.DTO.Interfaces;
using TimeSpanConverter = SmartCarWashTest.WebApi.DTO.Converters.TimeSpanConverter;

namespace SmartCarWashTest.WebApi.DTO.Models.Sale
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