using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.WebApi.DTOs.Interfaces;

namespace SmartCarWashTest.WebApi.DTOs.Models.SalesPoint
{
    /// <summary>
    /// Point of sale update model.
    /// </summary>
    /// <param name="Id">Point of sale ID.</param>
    /// <param name="Name">Name of the outlet.</param>
    public record SalesPointUpdateModel([Required] int Id, [Required] string Name) : IUpdateModel;
}