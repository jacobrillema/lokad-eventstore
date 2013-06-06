using System.IO;

namespace Lokad.EventStore.Core
{
    /// <summary>
    /// Delegate that writes pages to the underlying paged store.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <param name="source">The source.</param>
    public delegate void AppendWriterDelegate(int offset, Stream source);
}