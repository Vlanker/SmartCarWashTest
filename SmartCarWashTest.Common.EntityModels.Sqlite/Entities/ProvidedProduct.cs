using SmartCarWashTest.Common.EntityModels.Sqlite.Abstractions;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    // /// <summary>
    // /// Entity of the provided product.
    // /// </summary>
    /// <summary>
    /// Entity of the provided product.
    /// </summary>
    /// <param name="Id">Provided product ID.</param>
    /// <param name="ProductId">Product ID in entity of the provided product. FKey</param>
    /// <param name="ProductQuantity">Product quantity in entity of the provided product.</param>
    /// <param name="SalesPointId">Sales point ID in entity of the provided product.</param>
    public record ProvidedProduct(int Id, int ProductId, int ProductQuantity, int SalesPointId) : IHaveIdentifier
    {
        public ProvidedProduct() : this(default, default, default, default)
        {
        }

        // defines a navigation property for related rows.
        public virtual Product Product { get; set; } = null!;
        public virtual SalesPoint SalePoint { get; set; } = null!;
    }
}