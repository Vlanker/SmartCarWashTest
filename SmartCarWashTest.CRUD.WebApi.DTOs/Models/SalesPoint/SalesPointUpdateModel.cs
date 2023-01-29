using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesPoint
{
    /// <summary>
    /// Point of sale update model.
    /// </summary>
    /// <param name="Id">Point of sale ID.</param>
    /// <param name="Name">Name of the outlet.</param>
    public record SalesPointUpdateModel([property: Required] int Id, [property: Required] string Name) : IUpdateModel
    {
        public SalesPointUpdateModel() : this(default, string.Empty)
        {
        }
    }
}