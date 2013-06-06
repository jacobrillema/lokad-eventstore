#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System.Linq;
using Lokad.EventStore.Core;
using NUnit.Framework;

// ReSharper disable InconsistentNaming 

namespace Lokad.EventStore.Tests.Cache.LockingInMemoryCache
{
    [TestFixture]
    public class when_checking_store_version
    {
        [Test]
        public void given_empty_cache()
        {
            Assert.AreEqual(0, new EventStore.Cache.LockingInMemoryCache().StoreVersion);
        }

        [Test]
        public void given_cache_with_one_appended_record()
        {
            var cache = new EventStore.Cache.LockingInMemoryCache();
            cache.ConcurrentAppend("Stream", new byte[0], (version, storeVersion) => { }, -1);

            Assert.AreEqual(1, cache.StoreVersion);
        }

        [Test]
        public void given_empty_reload()
        {
            var cache = new EventStore.Cache.LockingInMemoryCache();
            cache.LoadHistory(Enumerable.Empty<StorageFrameDecoded>());
            Assert.AreEqual(0, cache.StoreVersion);
        }

        [Test]
        public void given_non_empty_reload()
        {
            var cache = new EventStore.Cache.LockingInMemoryCache();
            cache.LoadHistory(new[]
                {
                    new StorageFrameDecoded(new byte[1], "test", 0),
                });

            Assert.AreEqual(1, cache.StoreVersion);
        }
    }
}