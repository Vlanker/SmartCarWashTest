using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.WebApi.DTO.Interfaces;

namespace SmartCarWashTest.WebApi.DTO.Models.Buyer
{
    /// <summary>
    /// Buyer create model.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">Buyer name.</param>
    public record BuyerCreateModel(int Id, [Required] string Name) : ICreateModel;
}