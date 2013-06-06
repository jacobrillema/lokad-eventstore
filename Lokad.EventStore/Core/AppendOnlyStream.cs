#region (c) 2010-2013 Lokad EventStore - New BSD License 

// Copyright (c) Lokad 2010-2013 and contributors, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.IO;

namespace Lokad.EventStore.Core
{
    /// <summary>
    /// Helps to write data to the underlying store, which accepts only
    /// pages with specific size
    /// </summary>
    public sealed class AppendOnlyStream : IDisposable
    {
        readonly int _pageSizeInBytes;
        readonly AppendWriterDelegate _writer;
        readonly int _maxByteCount;
        MemoryStream _pending;

        int _bytesWritten;
        int _bytesPending;
        int _fullPagesFlushed;
        int _persistedPosition;


        public AppendOnlyStream(int pageSizeInBytes, AppendWriterDelegate writer, int maxByteCount)
        {
            _writer = writer;
            _maxByteCount = maxByteCount;
            _pageSizeInBytes = pageSizeInBytes;
            _pending = new MemoryStream();
        }

        public bool Fits(int byteCount)
        {
            return (_bytesWritten + byteCount <= _maxByteCount);
        }

        public void Write(byte[] buffer)
        {
            _pending.Write(buffer, 0, buffer.Length);
            _bytesWritten += buffer.Length;
            _bytesPending += buffer.Length;
        }

        public void Flush()
        {
            if (_bytesPending == 0)
                return;

            var size = (int) _pending.Length;
            var padSize = (_pageSizeInBytes - size % _pageSizeInBytes) % _pageSizeInBytes;

            using (var stream = new MemoryStream(size + padSize))
            {
                stream.Write(_pending.ToArray(), 0, (int) _pending.Length);
                if (padSize > 0)
                    stream.Write(new byte[padSize], 0, padSize);

                stream.Position = 0;
                _writer(_fullPagesFlushed * _pageSizeInBytes, stream);
            }

            var fullPagesFlushed = size / _pageSizeInBytes;

            if (fullPagesFlushed > 0)
            {
                // Copy remainder to the new stream and dispose the old stream
                var newStream = new MemoryStream();
                _pending.Position = fullPagesFlushed * _pageSizeInBytes;
                _pending.CopyTo(newStream);
                _pending.Dispose();
                _pending = newStream;
                _bytesPending = 0;
            }

            _fullPagesFlushed += fullPagesFlushed;
            _persistedPosition = _fullPagesFlushed * _pageSizeInBytes + (int) _pending.Length;
        }

        public int PersistedPosition
        {
            get { return _persistedPosition; }
        }

        public void Dispose()
        {
            Flush();
            _pending.Dispose();
        }
    }
}