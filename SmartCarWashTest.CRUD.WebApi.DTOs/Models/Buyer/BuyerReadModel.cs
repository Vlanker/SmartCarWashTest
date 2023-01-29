using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer
{
    /// <summary>
    /// Buyer read model.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">A collection of all Sale IDs.</param>
    /// <param name="SalesIds">List ID of sale</param>
    public record BuyerReadModel(int Id, string Name, [property: XmlIgnore] IEnumerable<int> SalesIds) : IReadModel,
        IHaveIdentifier
    {
        public BuyerReadModel() : this(default, string.Empty, Enumerable.Empty<int>())
        {
        }
    }
}