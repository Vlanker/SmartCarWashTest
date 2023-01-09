namespace SmartCarWashTest.Logger
{
    public abstract class LoggingEvents
    {
        public const int ListItems = 1001;
        public const int GetItem = 1002;
        public const int InsertItem = 1003;
        public const int UpdateItem = 1004;
        public const int DeleteItem = 1005;

        public const int UpdateItemNotContent = 2044;
        public const int DeleteItemNotContent = 2045;

        public const int GetItemNotFound = 4001;
        public const int UpdateItemNotFound = 4004;
        public const int DeleteItemNotFound = 4005;
        
        public const int InsertItemIsNullBadRequest = 40430;
        public const int InsertItemNotAddedBadRequest = 40431;

        public const int DeletedItemNotDeletedBadRequest = 4045;
            
        public const int UpdateItemIsNullOrDifferentIdBadRequest = 40440;
        public const int UpdateItemNotUpdatedBadRequest = 40441;
    }
}