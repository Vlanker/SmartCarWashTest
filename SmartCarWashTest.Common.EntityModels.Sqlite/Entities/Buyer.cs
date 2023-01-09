using System.Collections.Generic;
using SmartCarWashTest.Common.EntityModels.Sqlite.Interfaces;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Buyer entity.
    /// </summary>
    /// <param name="Id">Buyer ID.</param>
    /// <param name="Name">Buyer name.</param>
    public record Buyer(int Id, string Name) : IHaveIdentifier
    {
        // defines a navigation property for related rows
        public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}