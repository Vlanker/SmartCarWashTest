using System;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Sale entity.
    /// </summary>
    public class Sale
    {
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
        /// </summary>
        public int SalesPointId { get; set; }

        /// <summary>
        /// Buyer ID in Sale entity.
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// Total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}