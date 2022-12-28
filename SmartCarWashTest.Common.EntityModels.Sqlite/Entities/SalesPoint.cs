using System.Collections.Generic;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Point of sale entity.
    /// </summary>
    public class SalesPoint
    {
        public SalesPoint()
        {
            ProvidedProduct = new HashSet<ProvidedProduct>();
            Sales = new HashSet<Sale>();
        }

        /// <summary>
        /// Point of sale ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the outlet.
        /// </summary>
        public string Name { get; set; }

        // defines a navigation property for related rows.
        public virtual ICollection<ProvidedProduct> ProvidedProduct { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}