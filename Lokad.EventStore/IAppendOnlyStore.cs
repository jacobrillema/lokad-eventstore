#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;

namespace Lokad.EventStore
{
    public interface IAppendOnlyStore : IDisposable
    {
        /// <summary>
        /// <para>
        /// Appends data to the stream with the specified name. If <paramref name="expectedStreamVersion"/> is supplied and
        /// it does not match server version, then <see cref="AppendOnlyStoreConcurrencyException"/> is thrown.
        /// </para> 
        /// </summary>
        /// <param name="streamName">The name of the stream, to which data is appended.</param>
        /// <param name="data">The data to append.</param>
        /// <param name="expectedStreamVersion">The server version (supply -1 to append without check).</param>
        /// <exception cref="AppendOnlyStoreConcurrencyException">thrown when expected server version is
        /// supplied and does not match to server version</exception>
        void Append(string streamName, byte[] data, long expectedStreamVersion = -1);

        /// <summary>
        /// Reads the records by stream name.
        /// </summary>
        /// <param name="streamName">The key.</param>
        /// <param name="startingFrom">The after version.</param>
        /// <param name="maxCount">The max count.</param>
        /// <returns></returns>
        IEnumerable<DataWithKey> ReadRecords(string streamName, long startingFrom, int maxCount);

        /// <summary>
        /// Reads the records across all streams.
        /// </summary>
        /// <param name="startingFrom">The after version.</param>
        /// <param name="maxCount">The max count.</param>
        /// <returns></returns>
        IEnumerable<DataWithKey> ReadRecords(long startingFrom, int maxCount);

        void Close();
        void ResetStore();
        long GetCurrentVersion();
    }
}