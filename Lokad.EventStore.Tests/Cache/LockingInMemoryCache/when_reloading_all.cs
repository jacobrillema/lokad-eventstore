#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using NUnit.Framework;

namespace Lokad.EventStore.Tests.Cache.LockingInMemoryCache
{
    [TestFixture]
    public sealed class when_reloading_all : fixture_with_cache_helpers
    {
        [Test]
        public void given_reloaded_cache()
        {
            var cache = new EventStore.Cache.LockingInMemoryCache();
            cache.LoadHistory(CreateFrames("s1", "s2"));

            Assert.Throws<InvalidOperationException>(() => cache.LoadHistory(CreateFrames("s1")));
        }

        [Test]
        public void given_empty_cache()
        {
            var cache = new EventStore.Cache.LockingInMemoryCache();
            cache.Clear(() => { });
        }

        [Test]
        public void given_cleared_cache()
        {
            var cache = new EventStore.Cache.LockingInMemoryCache();
            cache.LoadHistory(CreateFrames("s1", "s2"));
            cache.Clear(() => { });
            cache.LoadHistory(CreateFrames("s1"));

            Assert.AreEqual(1, cache.StoreVersion, "storeVersion");
        }
    }
}