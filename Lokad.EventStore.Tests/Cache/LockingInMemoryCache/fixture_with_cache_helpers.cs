#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lokad.EventStore.Core;
using NUnit.Framework;

namespace Lokad.EventStore.Tests.Cache.LockingInMemoryCache
{
    public abstract class fixture_with_cache_helpers
    {
        protected IEnumerable<StorageFrameDecoded> CreateFrames(params string[] streamNames)
        {
            for (int i = 0; i < streamNames.Length; i++)
            {
                var streamName = streamNames[i];
                var storeVersion = i + 1;
                var bytes = Encoding.UTF8.GetBytes("event-" + storeVersion);
                yield return new StorageFrameDecoded(bytes, streamName, ("event-" + storeVersion).GetHashCode());
            }
        }

        protected void EatException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception) {}
        }

        public static class DataAssert
        {
            public static void AreEqual(IEnumerable<DataWithKey> expected, IEnumerable<DataWithKey> actual)
            {
                var ea = expected.ToArray();
                var aa = actual.ToArray();


                Assert.AreEqual(ea.Length, aa.Length, "Array length");

                for (int i = 0; i < ea.Length; i++)
                {
                    var expectedItem = ea[i];
                    var actualItem = aa[i];

                    Assert.AreEqual(expectedItem.Key, actualItem.Key, "Item[{0}].Key", i);
                    Assert.AreEqual(expectedItem.StoreVersion, actualItem.StoreVersion, "Item[{0}].StoreVersion", i);
                    Assert.AreEqual(expectedItem.StreamVersion, actualItem.StreamVersion, "Item[{0}].StreamVersion", i);
                    CollectionAssert.AreEqual(expectedItem.Data, actualItem.Data, "Item[{0}].Data", i);
                }
            }
        }

        protected DataWithKey CreateKey(int storeVersion, int streamVersion, string streamName)
        {
            var bytes = GetEventBytes(storeVersion);
            return new DataWithKey(streamName, bytes, streamVersion, storeVersion);
        }

        protected byte[] GetEventBytes(int storeVersion)
        {
            return Encoding.UTF8.GetBytes("event-" + storeVersion);
        }
    }
}