using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.SalesPoint
{
    /// <summary>
    /// Point of sale create model.
    /// </summary>
    /// <param name="Name">Name of the outlet.</param>
    public record SalesPointCreateModel([property: Required] string Name) : ICreateModel
    {
        public SalesPointCreateModel() : this(string.Empty)
        {
        }
    }
}