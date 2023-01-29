using System.ComponentModel.DataAnnotations;
using SmartCarWashTest.CRUD.WebApi.DTOs.Abstractions;

namespace SmartCarWashTest.CRUD.WebApi.DTOs.Models.Buyer
{
    /// <summary>
    /// Buyer update model.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">Buyer name.</param>
    public record BuyerUpdateModel([property: Required] int Id, string Name) : IUpdateModel
    {
        public BuyerUpdateModel() : this(default, string.Empty)
        {
        }
    }
}