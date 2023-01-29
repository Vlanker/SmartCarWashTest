using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct
{
    /// <summary>
    /// Create model of the provided product.
    /// </summary>
    /// <param name="ProductId">Product ID in model of the provided product.</param>
    /// <param name="ProductQuantity">Product quantity in model of the provided product.</param>
    /// <param name="SalesPointId">Sales point ID in model of the provided product.</param>
    public record ProvidedProductCreateModel([property: Required] int ProductId,
        [property: Required] int ProductQuantity,
        [property: Required] int SalesPointId) : ICreateModel
    {
        public ProvidedProductCreateModel() : this(default, default, default)
        {
        }
    }
}