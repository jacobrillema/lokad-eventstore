using System.Collections.Generic;
using System.IO;

namespace Lokad.EventStore
{
    /// <summary>
    /// Helper class that knows how to store arbitrary messages in append-only store
    /// (including envelopes, audit batches etc)
    /// </summary>
    public class MessageStore
    {
        readonly IAppendOnlyStore _appendOnlyStore;
        readonly IMessageSerializer _serializer;

        public void Dispose()
        {
            _appendOnlyStore.Close();
            _appendOnlyStore.Dispose();
        }

        public MessageStore(IAppendOnlyStore appendOnlyStore, IMessageSerializer serializer)
        {
            _appendOnlyStore = appendOnlyStore;
            _serializer = serializer;
        }

        public IEnumerable<StoreRecord> EnumerateMessages(string key, long afterVersion, int count)
        {
            var records = _appendOnlyStore.ReadRecords(key, afterVersion, count);
            foreach (var record in records)
            {
                using (var mem = new MemoryStream(record.Data))
                {
                    // drop attributes
                    var attribs = _serializer.ReadAttributes(mem);
                    var eventCount = _serializer.ReadCompactInt(mem);
                    var objects = new object[eventCount];
                    for (int i = 0; i < eventCount; i++)
                    {
                        objects[i] = _serializer.ReadMessage(mem);
                    }
                    yield return new StoreRecord(key, objects, record.StoreVersion, record.StreamVersion, attribs);
                }
            }
        }

        public long GetVersion()
        {
            return _appendOnlyStore.GetCurrentVersion();
        }


        public IEnumerable<StoreRecord> EnumerateAllItems(long afterVersion, int take)
        {
            // we don't use any index = just skip all audit things
            foreach (var record in _appendOnlyStore.ReadRecords(afterVersion, take))
            {
                using (var mem = new MemoryStream(record.Data))
                {
                    // ignore the attributes here
                    var attribs = _serializer.ReadAttributes(mem);
                    var count = _serializer.ReadCompactInt(mem);
                    var result = new object[count];
                    for (int i = 0; i < count; i++)
                    {
                        result[i] = _serializer.ReadMessage(mem);
                    }
                    yield return new StoreRecord(record.Key, result, record.StoreVersion, record.StreamVersion, attribs);
                }
            }
        }

        public void AppendToStore(string name, ICollection<MessageAttribute> attribs, long streamVersion, ICollection<object> messages)
        {
            using (var mem = new MemoryStream())
            {
                _serializer.WriteAttributes(attribs, mem);
                _serializer.WriteCompactInt(messages.Count, mem);
                foreach (var message in messages)
                {
                    _serializer.WriteMessage(message, message.GetType(), mem);
                }
                _appendOnlyStore.Append(name, mem.ToArray(), streamVersion);
            }
        }
    }
}