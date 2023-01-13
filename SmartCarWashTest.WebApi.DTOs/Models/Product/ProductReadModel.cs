using SmartCarWashTest.WebApi.DTOs.Interfaces;

namespace SmartCarWashTest.WebApi.DTOs.Models.Product
{
    /// <summary>
    /// Product read model.
    /// </summary>
    /// <param name="Id">Product ID.</param>
    /// <param name="Name">Product name.</param>
    /// <param name="Price">Product price.</param>
    public record ProductReadModel(int Id, string Name, decimal Price) : IReadModel, IHaveIdentifier;
}