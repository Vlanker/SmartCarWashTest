using System;
using System.Collections.Generic;
using SmartCarWashTest.Common.EntityModels.Sqlite.Abstractions;

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
        public Product() : this(default, String.Empty, default)
        {
            ProvidedProducts = new HashSet<ProvidedProduct>();
            SalesDataSet = new HashSet<SalesData>();
        }

        // defines a navigation property for related rows
        public virtual ICollection<ProvidedProduct> ProvidedProducts { get; set; }
        public virtual ICollection<SalesData> SalesDataSet { get; set; }
    }
}