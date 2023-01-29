using Microsoft.Extensions.Logging;

namespace SmartCarWashTest.Logger.Events
{
    public static class LoggingEventIds
    {
        public static readonly EventId ListItems = new(1000, "Read List");
        public static readonly EventId GetItem = new(1001, "Read");
        public static readonly EventId InsertItem = new(1002, "Inserted");
        public static readonly EventId UpdateItem = new(1003, "Updated");
        public static readonly EventId DeleteItem = new(1004, "Deleted");
        
        public const int Details = 3000;
        public const int Error = 3001;

        public static readonly EventId UpdateItemNotContent = 2040;
        public static readonly EventId DeleteItemNotContent = 2041;

        public static readonly EventId GetItemNotFound = 4000;
        public static readonly EventId UpdateItemNotFound = 4001;
        public static readonly EventId DeleteItemNotFound = 4002;
        
        public static readonly EventId InsertItemIsNullBadRequest = 4040;
        public static readonly EventId InsertItemNotAddedBadRequest = 4041;
        public static readonly EventId DeletedItemNotDeletedBadRequest = 4042;
        public static readonly EventId UpdateItemIsNullOrDifferentIdBadRequest = 4043;
        public static readonly EventId UpdateItemNotUpdatedBadRequest = 4044;
    }
}