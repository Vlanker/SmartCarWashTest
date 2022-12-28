using System;
using System.Collections.Generic;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Sale entity.
    /// </summary>
    public class Sale
    {
        public Sale()
        {
            SalesDataSet = new HashSet<SalesData>();
        }

        /// <summary>
        /// Sale ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date of sale.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Time of sale.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Point of sale ID in Sale entity.
        /// FKey
        /// </summary>
        public int SalesPointId { get; set; }

        /// <summary>
        /// Buyer ID in Sale entity.
        /// FKey
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// Total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        // defines a navigation property for related rows.
        public virtual Buyer Buyer { get; set; }
        public virtual SalesPoint SalesPoint { get; set; }
        public virtual ICollection<SalesData> SalesDataSet { get; set; }
    }
}