#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System.Collections.Generic;
using Lokad.EventStore.Cache;

namespace Lokad.EventStore.Memory
{
    public sealed class MemoryAppendOnlyStore : IAppendOnlyStore
    {
        readonly LockingInMemoryCache _cache = new LockingInMemoryCache();


        public void InitializeForWriting() {}

        public void Append(string streamName, byte[] data, long expectedStreamVersion = -1)
        {
            _cache.ConcurrentAppend(streamName, data, (version, storeVersion) => { }, expectedStreamVersion);
        }

        public IEnumerable<DataWithKey> ReadRecords(string streamName, long startingFrom, int maxCount)
        {
            return _cache.ReadStream(streamName, startingFrom, maxCount);
        }

        public IEnumerable<DataWithKey> ReadRecords(long startingFrom, int maxCount)
        {
            return _cache.ReadAll(startingFrom, maxCount);
        }

        public void Close() {}

        public void ResetStore()
        {
            _cache.Clear(() => { });
        }

        public long GetCurrentVersion()
        {
            return _cache.StoreVersion;
        }

        public void Dispose() {}
    }
}