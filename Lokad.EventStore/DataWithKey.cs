using System;

namespace Lokad.EventStore
{
    public sealed class DataWithKey
    {
        public readonly string Key;
        public readonly byte[] Data;
        public readonly long StreamVersion;
        public readonly long StoreVersion;

        public DataWithKey(string key, byte[] data, long streamVersion, long storeVersion)
        {
            if (null == data)
                throw new ArgumentNullException("data");
            Key = key;
            Data = data;
            StreamVersion = streamVersion;
            StoreVersion = storeVersion;
        }
    }
}