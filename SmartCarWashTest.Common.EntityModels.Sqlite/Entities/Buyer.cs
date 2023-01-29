using System.Collections.Generic;
using SmartCarWashTest.Common.EntityModels.Sqlite.Abstractions;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Buyer entity.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">Buyer name.</param>
    public record Buyer(int Id, string Name) : IHaveIdentifier
    {
        public Buyer() : this(default, string.Empty)
        {
            Sales = new HashSet<Sale>();
        }

        // defines a navigation property for related rows
        public virtual ICollection<Sale> Sales { get; set; }
    }
}