namespace SmartCarWashTest.Common.EntityModels.Sqlite.Entities
{
    /// <summary>
    /// Product entity.
    /// </summary>
    public class Product
    {
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
    }
}