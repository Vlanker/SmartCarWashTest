using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.WebApi.DTOs.Interfaces;

namespace SmartCarWashTest.WebApi.DTOs.Models.Buyer
{
    /// <summary>
    /// Buyer update model.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">Buyer name.</param>
    public record BuyerUpdateModel([Required] int Id, string Name) : IUpdateModel;
}