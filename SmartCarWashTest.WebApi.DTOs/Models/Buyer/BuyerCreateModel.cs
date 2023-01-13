using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.WebApi.DTOs.Interfaces;

namespace SmartCarWashTest.WebApi.DTOs.Models.Buyer
{
    /// <summary>
    /// Buyer create model.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">Buyer name.</param>
    public record BuyerCreateModel(int Id, [Required] string Name) : ICreateModel;
}