using System;
using System.Collections.Generic;
using System.IO;

namespace Lokad.EventStore
{
    /// <summary>
    /// Joins data serializer and contract mapper
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// Writes the message to a stream.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        /// <param name="stream">The stream.</param>
        void WriteMessage(object message, Type type, Stream stream);
        /// <summary>
        /// Reads the message from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        object ReadMessage(Stream stream);

        MessageAttribute[] ReadAttributes(Stream stream);
        void WriteAttributes(ICollection<MessageAttribute> attributes, Stream stream);

        int ReadCompactInt(Stream stream);
        void WriteCompactInt(int value, Stream steam);
    }
}