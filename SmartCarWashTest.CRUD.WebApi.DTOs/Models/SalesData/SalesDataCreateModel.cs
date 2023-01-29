using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData
{
    /// <summary>
    ///  Sale data create model.
    /// </summary>
    /// <param name="ProductId">Product ID in Sale data model.</param>
    /// <param name="ProductQuantity">Product quantity in Sale data model.</param>
    /// <param name="ProductIdAmount">The total cost of the purchased quantity of goods of the given Product ID in Sale data model.</param>
    /// <param name="SaleId">Sale ID in Sale data model.</param>
    public record SalesDataCreateModel([property: Required] int ProductId, [property: Required] int ProductQuantity,
        [property: Required] int ProductIdAmount, [property: Required] int SaleId) : ICreateModel
    {
        public SalesDataCreateModel() : this(default, default, default, default)
        {
        }
    }
}