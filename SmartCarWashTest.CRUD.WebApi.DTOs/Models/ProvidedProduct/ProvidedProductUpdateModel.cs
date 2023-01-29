using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct
{
    /// <summary>
    /// Update model of the provided product.
    /// </summary>
    /// <param name="Id">Provided product ID.</param>
    /// <param name="ProductId">Product ID in model of the provided product.</param>
    /// <param name="ProductQuantity">Product quantity in model of the provided product.</param>
    /// <param name="SalesPointId">Sales point ID in model of the provided product.</param>
    public record ProvidedProductUpdateModel([property: Required] int Id, int ProductId, int ProductQuantity,
        int SalesPointId) : IUpdateModel
    {
        public ProvidedProductUpdateModel() : this(default, default, default, default)
        {
        }
    }
}