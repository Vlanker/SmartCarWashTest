namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Sale data entity.
    /// </summary>
    public class SalesData
    {
        /// <summary>
        /// Sale data ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product ID in Sale data entity.
        /// FKey
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Product quantity in Sale data entity.
        /// </summary>
        public int ProductQuantity { get; set; }

        /// <summary>
        /// The total cost of the purchased quantity of goods of the given Product ID in Sale data entity.
        /// </summary>
        public int ProductIdAmount { get; set; }

        /// <summary>
        /// Sale ID in Sale data entity.
        /// FKey
        /// </summary>
        public int SaleId { get; set; }

        // defines a navigation property for related rows.
        public virtual Product Product { get; set; }
        public virtual Sale Sale { get; set; }
    }
}