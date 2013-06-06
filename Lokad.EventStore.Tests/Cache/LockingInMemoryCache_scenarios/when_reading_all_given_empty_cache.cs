#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using Lokad.EventStore.Cache;
using NUnit.Framework;

namespace Lokad.EventStore.Tests.Cache.LockingInMemoryCache_scenarios
{
    [TestFixture]
    public sealed class when_reading_all_given_empty_cache : fixture_with_cache_helpers
    {
        LockingInMemoryCache Cache;

        [SetUp]
        public void Setup()
        {
            Cache = new LockingInMemoryCache();
        }

        [Test]
        public void given_full_range()
        {
            CollectionAssert.IsEmpty(Cache.ReadAll(0, int.MaxValue));
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