using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.WebApi.DTO.Interfaces;

namespace SmartCarWashTest.WebApi.DTO.Models.SalesData
{
    /// <summary>
    /// Sale data update model.
    /// </summary>
    /// <param name="Id">Sale data ID.</param>
    /// <param name="ProductId">Product ID in Sale data model.</param>
    /// <param name="ProductQuantity">Product quantity in Sale data model.</param>
    /// <param name="ProductIdAmount">
    /// The total cost of the purchased quantity of goods of the given Product ID in Sale data model.
    /// </param>
    /// <param name="SaleId">Sale ID in Sale data model.</param>
    public record SalesDataUpdateModel([Required] int Id, int ProductId, int ProductQuantity, int ProductIdAmount,
        int SaleId) : IUpdateModel;
}