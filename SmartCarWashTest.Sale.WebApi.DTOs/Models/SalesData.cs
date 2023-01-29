using System.ComponentModel.DataAnnotations;

namespace SmartCarWashTest.Sale.WebApi.DTOs.Models
{
    /// <summary>
    /// Sales data.
    /// </summary>
    /// <param name="ProductId">Product ID.</param>
    /// <param name="ProductQuantity">Product quantity.</param>
    /// <param name="ProductIdAmount">ProductIdAmount.</param>
    public record SalesData([property: Required] int ProductId, [property: Required] int ProductQuantity,
        [property: Required] int ProductIdAmount)
    {
        public SalesData() : this(0, 0, 0)
        {
        }
    }
}