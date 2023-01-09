using System.Collections.Generic;
using System.Xml.Serialization;
using SmartCarWashTest.WebApi.DTO.Interfaces;

namespace SmartCarWashTest.WebApi.DTO.Models.Buyer
{
    /// <summary>
    /// Buyer read model.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">A collection of all Sale IDs.</param>
    /// <param name="SalesIds">List ID of sale</param>
    public record BuyerReadModel(int Id, string Name, [XmlIgnore] IEnumerable<int> SalesIds) :
        IReadModel,
        IHaveIdentifier;
}