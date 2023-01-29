using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.Product
{
    /// <summary>
    /// Product update model.
    /// </summary>
    /// <param name="Id">Product ID.</param>
    /// <param name="Name">Product name.</param>
    /// <param name="Price">Product price.</param>
    public record ProductUpdateModel([property: Required] int Id, string Name, decimal Price) : IUpdateModel
    {
        public ProductUpdateModel() : this(default, string.Empty, default)
        {
        }
    }
}