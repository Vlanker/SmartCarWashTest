namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Point of sale entity.
    /// </summary>
    public class SalesPoint
    {
        /// <summary>
        /// Point of sale ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the outlet.
        /// </summary>
        public string Name { get; set; }
    }
}