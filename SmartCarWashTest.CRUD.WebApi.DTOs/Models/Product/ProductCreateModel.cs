using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.Product
{
    /// <summary>
    /// Product create model.
    /// </summary>
    /// <param name="Name">Product name.</param>
    /// <param name="Price">Product price.</param>
    public record ProductCreateModel([property: Required] string Name,
        [property: Required] decimal Price) : ICreateModel
    {
        public ProductCreateModel() : this(string.Empty, default)
        {
        }
    }
}