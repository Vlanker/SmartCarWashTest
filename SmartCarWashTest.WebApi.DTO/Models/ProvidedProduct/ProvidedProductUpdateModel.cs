using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.WebApi.DTO.Interfaces;

namespace SmartCarWashTest.WebApi.DTO.Models.ProvidedProduct
{
    /// <summary>
    /// Update model of the provided product.
    /// </summary>
    /// <param name="Id">Provided product ID.</param>
    /// <param name="ProductId">Product ID in model of the provided product.</param>
    /// <param name="ProductQuantity">Product quantity in model of the provided product.</param>
    /// <param name="SalesPointId">Sales point ID in model of the provided product.</param>
    public record ProvidedProductUpdateModel([Required] int Id, int ProductId, int ProductQuantity, int SalesPointId) :
        IUpdateModel;
}