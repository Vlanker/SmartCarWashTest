using SmartCarWashTest.Common.EntityModels.Sqlite.Interfaces;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Sale data entity.
    /// </summary>
    /// <param name="Id">Sale data ID.</param>
    /// <param name="ProductId">Product ID in Sale data entity.</param>
    /// <param name="ProductQuantity">Product quantity in Sale data entity.</param>
    /// <param name="ProductIdAmount">
    /// The total cost of the purchased quantity of goods of the given Product ID in Sale data entity.
    /// </param>
    /// <param name="SaleId">Sale ID in Sale data entity. FKey</param>
    public record SalesData(int Id, int ProductId, int ProductQuantity, int ProductIdAmount, int SaleId) :
        IHaveIdentifier
    {
        // defines a navigation property for related rows.
        public virtual Product Product { get; set; } = null!;
        public virtual Sale Sale { get; set; } = null!;
    }
}