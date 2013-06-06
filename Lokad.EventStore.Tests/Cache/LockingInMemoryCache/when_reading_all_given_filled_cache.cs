#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using NUnit.Framework;

namespace Lokad.EventStore.Tests.Cache.LockingInMemoryCache
{
    [TestFixture]
    public sealed class when_reading_all_given_filled_cache : fixture_with_cache_helpers
    {
        EventStore.Cache.LockingInMemoryCache Cache;

        [SetUp]
        public void Setup()
        {
            Cache = new EventStore.Cache.LockingInMemoryCache();

            Cache.LoadHistory(CreateFrames("stream1", "stream2"));
            Cache.ConcurrentAppend("stream1", GetEventBytes(3), (version, storeVersion) => { });
        }

        [Test]
        public void given_non_matching_range()
        {
            CollectionAssert.IsEmpty(Cache.ReadAll(3, 10));
        }

        [Test]
        public void given_intersecting_range()
        {
            var dataWithKeys = Cache.ReadAll(1, 1);
            DataAssert.AreEqual(new[] {CreateKey(2, 1, "stream2")}, dataWithKeys);
        }


        [Test]
        public void given_matching_range()
        {
            var dataWithKeys = Cache.ReadAll(0, 3);
            DataAssert.AreEqual(new[]
                {
                    CreateKey(1, 1, "stream1"),
                    CreateKey(2, 1, "stream2"),
                    CreateKey(3, 2, "stream1")
                }, dataWithKeys);
        }

        [Test]
        public void given_full_range()
        {
            var dataWithKeys = Cache.ReadAll(0, int.MaxValue);
            DataAssert.AreEqual(new[]
                {
                    CreateKey(1, 1, "stream1"),
                    CreateKey(2, 1, "stream2"),
                    CreateKey(3, 2, "stream1")
                }, dataWithKeys);
        }

        [Test]
        public void given_negative_store_version()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Cache.ReadAll(-1, int.MaxValue));
        }

        [Test]
        public void given_zero_count()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Cache.ReadAll(0, 0));
        }
    }
}