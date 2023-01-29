using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;
using SmartCarWashTest.CRUD.WebApi.DTOs.Models.ProvidedProduct;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesPoint
{
    /// <summary>
    /// Point of sale read model.
    /// </summary>
    /// <param name="Id">Point of sale ID.</param>
    /// <param name="Name">Name of the outlet.</param>
    public record SalesPointReadModel(int Id, string Name,
        [property: XmlIgnore] IEnumerable<ProvidedProductReadModel> ProvidedProducts) : IReadModel, IHaveIdentifier
    {
        public SalesPointReadModel() : this(default, string.Empty, Enumerable.Empty<ProvidedProductReadModel>())
        {
        }
    }
}