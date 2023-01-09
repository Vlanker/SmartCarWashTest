using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.WebApi.DTO.Interfaces;

namespace SmartCarWashTest.WebApi.DTO.Models.Product
{
    /// <summary>
    /// Product update model.
    /// </summary>
    /// <param name="Id">Product ID.</param>
    /// <param name="Name">Product name.</param>
    /// <param name="Price">Product price.</param>
    public record ProductUpdateModel([Required] int Id, string Name, decimal Price) : IUpdateModel;
}