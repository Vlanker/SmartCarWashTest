using System.Collections.Generic;
using SmartCarWashTest.Common.EntityModels.Sqlite.Abstractions;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Point of sale entity.
    /// </summary>
    /// <param name="Id">Point of sale ID.</param>
    /// <param name="Name">Name of the outlet.</param>
    public record SalesPoint(int Id, string Name) : IHaveIdentifier
    {
        public SalesPoint() : this(default, default)
        {
            ProvidedProduct = new HashSet<ProvidedProduct>();
            Sales = new HashSet<Sale>();
        }

        // defines a navigation property for related rows.
        public virtual ICollection<ProvidedProduct> ProvidedProduct { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}