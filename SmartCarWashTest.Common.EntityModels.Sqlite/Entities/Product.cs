using System.Collections;
using System.Collections.Generic;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Product entity.
    /// </summary>
    public class Product
    {
        public Product()
        {
            ProvidedProducts = new HashSet<ProvidedProduct>();
            SalesDataSet = new HashSet<SalesData>();
        }

        /// <summary>
        /// Product ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product price.
        /// </summary>
        public decimal Price { get; set; }

        // defines a navigation property for related rows
        public virtual ICollection<ProvidedProduct> ProvidedProducts { get; set; }
        public virtual ICollection<SalesData> SalesDataSet { get; set; }
    }
}