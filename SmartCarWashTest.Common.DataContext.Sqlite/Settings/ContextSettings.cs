namespace SmartCarWashTest.Common.DataContext.Sqlite.Settings
{
    /// <summary>
    /// Context settings.
    /// </summary>
    /// <param name="DataProvider">Data provider.</param>
    /// <param name="DbRelativePath">DB relative path.</param>
    public sealed record ContextSettings(string DataProvider, string DbRelativePath)
    {
        public ContextSettings() : this(string.Empty, string.Empty)
        {
        }
    }
}