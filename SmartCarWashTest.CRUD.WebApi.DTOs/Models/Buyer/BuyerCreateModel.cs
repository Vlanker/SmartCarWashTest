using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer
{
    /// <summary>
    /// Buyer create model.
    /// </summary>
    /// <param name="Name">Buyer name.</param>
    public record BuyerCreateModel([property: Required] string Name) : ICreateModel
    {
        public BuyerCreateModel() : this(string.Empty)
        {
        }
    }
}