using System.Collections.Generic;
using SmartCarWashTest.Common.EntityModels.Sqlite.Interfaces;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Product entity.
    /// </summary>
    /// <param name="Id">Product ID.</param>
    /// <param name="Name">Product name.</param>
    /// <param name="Price">Product price.</param>
    public record Product(int Id, string Name, decimal Price) : IHaveIdentifier
    {
        // defines a navigation property for related rows
        public virtual ICollection<ProvidedProduct> ProvidedProducts { get; set; } = new HashSet<ProvidedProduct>();
        public virtual ICollection<SalesData> SalesDataSet { get; set; } = new HashSet<SalesData>();
    }
}