using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SmartCarWashTest.Common.Converters.JsonConverters;
using SmartCarWashTest.WebApi.DTOs.Interfaces;

namespace SmartCarWashTest.WebApi.DTOs.Models.Sale
{
    /// <summary>
    /// Sale create model.
    /// </summary>
    /// <param name="Id">Sale ID.</param>
    /// <param name="SalesPointId">Point of sale ID in Sale model.</param>
    /// <param name="BuyerId">Buyer ID in Sale model.</param>
    /// <param name="TotalAmount">Total amount.</param>
    public record SaleCreateModel(int Id, [Required] int SalesPointId, [Required] int BuyerId,
        [Required] decimal TotalAmount) : ICreateModel
    {
        /// <summary>
        /// Date of sale.
        /// </summary>
        [Required]
        [JsonConverter(typeof(DateTimeToDateConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Time of sale.
        /// </summary>
        [Required]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Time { get; set; }
    }
}