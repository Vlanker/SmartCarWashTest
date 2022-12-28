using System.Collections.Generic;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Buyer entity.
    /// </summary>
    public class Buyer
    {
        public Buyer()
        {
            Sales = new HashSet<Sale>();
        }

        /// <summary>
        /// Buyer ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Buyer name.
        /// </summary>
        public string Name { get; set; }

        // defines a navigation property for related rows
        public virtual ICollection<Sale> Sales { get; set; }
    }
}