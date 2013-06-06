using System;
using System.Runtime.Serialization;

namespace Lokad.EventStore.Core
{
    /// <summary>
    /// Is thrown when there is a big problem with reading storage frame
    /// </summary>
    [Serializable]
    public class StorageFrameException : Exception
    {
        public StorageFrameException(string message) : base(message) {}

        protected StorageFrameException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) {}
    }
}