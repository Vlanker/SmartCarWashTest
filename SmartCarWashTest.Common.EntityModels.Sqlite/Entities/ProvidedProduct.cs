namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Entity of the provided product.
    /// </summary>
    public class ProvidedProduct
    {
        /// <summary>
        /// Provided product ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product ID in entity of the provided product.
        /// FKey
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Product quantity in entity of the provided product.
        /// </summary>
        public int ProductQuantity { get; set; }

        /// <summary>
        /// Sales point ID in entity of the provided product.
        /// FKey
        /// </summary>
        public int SalesPointId { get; set; }

        // defines a navigation property for related rows.
        public virtual Product Product { get; set; }
        public virtual SalesPoint SalePoint { get; set; }
    }
}