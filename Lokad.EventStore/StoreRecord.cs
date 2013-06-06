namespace Lokad.EventStore
{
    public struct StoreRecord
    {
        public readonly object[] Items;
        public readonly long StoreVersion;
        public readonly long StreamVersion;
        public readonly string Key;
        public readonly MessageAttribute[] Attributes;

        public StoreRecord(string key, object[] items, long storeVersion, long streamVersion, MessageAttribute[] attributes)
        {
            Items = items;
            StoreVersion = storeVersion;
            StreamVersion = streamVersion;
            Key = key;
            Attributes = attributes;
        }
    }
}