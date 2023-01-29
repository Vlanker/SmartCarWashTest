using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesData
{
    /// <summary>
    /// Sale data read model.
    /// </summary>
    /// <param name="Id">Sale data ID.</param>
    /// <param name="ProductId">Product ID in Sale data model.</param>
    /// <param name="ProductQuantity">Product quantity in Sale data model.</param>
    /// <param name="ProductIdAmount">
    /// The total cost of the purchased quantity of goods of the given Product ID in Sale data model.
    /// </param>
    /// <param name="SaleId">Sale ID in Sale data model.</param>
    public record SalesDataReadModel(int Id, int ProductId, int ProductQuantity, int ProductIdAmount,
        int SaleId) : IReadModel, IHaveIdentifier
    {
        public SalesDataReadModel() : this(default, default, default, default, default)
        {
        }
    }
}