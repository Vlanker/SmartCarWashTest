using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.WebApi.DTO.Interfaces;

namespace SmartCarWashTest.WebApi.DTO.Models.Buyer
{
    /// <summary>
    /// Buyer update model.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">Buyer name.</param>
    public record BuyerUpdateModel([Required] int Id, string Name) : IUpdateModel;
}