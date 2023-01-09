using SmartCarWashTest.WebApi.DTO.Interfaces;

namespace SmartCarWashTest.WebApi.DTO.Models.ProvidedProduct
{
    /// <summary>
    /// Read model of the provided product.
    /// </summary>
    /// <param name="Id">Provided product ID.</param>
    /// <param name="ProductId">Product quantity in model of the provided product.</param>
    /// <param name="ProductQuantity">Product quantity in model of the provided product.</param>
    /// <param name="SalesPointId">Sales point ID in model of the provided product.</param>
    public record ProvidedProductReadModel(int Id, int ProductId, int ProductQuantity, int SalesPointId) : IReadModel,
        IHaveIdentifier;
}