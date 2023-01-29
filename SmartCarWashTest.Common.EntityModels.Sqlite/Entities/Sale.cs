using System;
using System.Collections.Generic;
using SmartCarWashTest.Common.EntityModels.Sqlite.Abstractions;

namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Sale entity.
    /// </summary>
    /// <param name="Id">Sale ID.</param>
    /// <param name="Date">Date of sale.</param>
    /// <param name="Time">Time of sale.</param>
    /// <param name="SalesPointId">Point of sale ID in Sale entity.</param>
    /// <param name="BuyerId">Buyer ID in Sale entity. FKey</param>
    /// <param name="TotalAmount">Total amount.</param>
    public record Sale(int Id, DateTime Date, TimeSpan Time, int SalesPointId, int? BuyerId, decimal TotalAmount) :
        IHaveIdentifier
    {
        public Sale() : this(default, DateTime.MinValue.Date, TimeSpan.Zero, default, default, default)
        {
            SalesDataSet = new HashSet<SalesData>();
        }

        // defines a navigation property for related rows.
        public virtual Buyer Buyer { get; set; } = null!;
        public virtual SalesPoint SalesPoint { get; set; } = null!;
        public virtual ICollection<SalesData> SalesDataSet { get; set; }
    }
}