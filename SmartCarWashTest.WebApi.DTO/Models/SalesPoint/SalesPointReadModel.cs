using System.Collections.Generic;
using System.Xml.Serialization;
using SmartCarWashTest.WebApi.DTO.Interfaces;
using SmartCarWashTest.WebApi.DTO.Models.ProvidedProduct;

namespace SmartCarWashTest.WebApi.DTO.Models.SalesPoint
{
    /// <summary>
    /// Point of sale read model.
    /// </summary>
    /// <param name="Id">Point of sale ID.</param>
    /// <param name="Name">Name of the outlet.</param>
    public record SalesPointReadModel(int Id, string Name,
            [XmlIgnore] IEnumerable<ProvidedProductReadModel> ProvidedProducts) : IReadModel, IHaveIdentifier;
}