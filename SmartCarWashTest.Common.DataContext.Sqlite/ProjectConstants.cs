namespace SmartCarWashTest.Common.DataContext.Sqlite
{
    public static class ProjectConstants
    {
        public const string DatabaseProvider = "SQLite";
        public const string ConnectionString = "SmartCarWashContext";
        public const string DbName = "SmartCarWash.db";
        public const string DefaultConnection = "Filename=../SmartCarWash.db";
        public const string MigrationProject = "SmartCarWashTest.Common.SeparateMigrations";
    }
}