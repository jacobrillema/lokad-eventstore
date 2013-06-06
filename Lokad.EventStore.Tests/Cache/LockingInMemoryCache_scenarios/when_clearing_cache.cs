#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System.IO;
using Lokad.EventStore.Cache;
using NUnit.Framework;

namespace Lokad.EventStore.Tests.Cache.LockingInMemoryCache_scenarios
{
    [TestFixture]
    public sealed class when_clearing_cache : fixture_with_cache_helpers
    {
        [Test]
        public void given_empty_cache()
        {
            var cache = new LockingInMemoryCache();
            cache.Clear(() => { });
            Assert.AreEqual(0, cache.StoreVersion);
        }

        [Test]
        public void given_reloaded_cache()
        {
            var cache = new LockingInMemoryCache();
            cache.LoadHistory(CreateFrames("stream2"));
            cache.Clear(() => { });

            Assert.AreEqual(0, cache.StoreVersion);
        }


        [Test]
        public void given_appended_cache()
        {
            var cache = new LockingInMemoryCache();

            cache.ConcurrentAppend("stream1", new byte[1], (version, storeVersion) => { });

            cache.Clear(() => { });

            Assert.AreEqual(0, cache.StoreVersion);
        }

        [Test]
        public void given_filled_cache_and_failing_commit_function()
        {
            var cache = new LockingInMemoryCache();

            cache.ConcurrentAppend("stream1", new byte[1], (version, storeVersion) => { });

            Assert.Throws<FileNotFoundException>(() => cache.Clear(() => { throw new FileNotFoundException(); }));

            Assert.AreEqual(1, cache.StoreVersion);
        }
    }
}