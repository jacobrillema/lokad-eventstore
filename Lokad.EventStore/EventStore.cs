using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace Lokad.EventStore
{
    public class EventStore
    {
        readonly SerializingAppendOnlyStore _store;

        public EventStore(SerializingAppendOnlyStore store)
        {
            _store = store;
        }

        public void AppendEventsToStream(string streamId, long streamVersion, ICollection<object> events)
        {
            if (events.Count == 0) return;

            try
            {
                _store.AppendToStore(streamId, MessageAttribute.Empty, streamVersion, events);
            }
            catch (AppendOnlyStoreConcurrencyException e)
            {
                // load server events
                var server = LoadEventStream(streamId);
                // throw a real problem
                throw OptimisticConcurrencyException.Create(server.StreamVersion, e.ExpectedStreamVersion, streamId, server.Events);
            }
        }


        public EventStream LoadEventStream(string streamId)
        {
            // TODO: make this lazy somehow?
            var stream = new EventStream();
            foreach (var record in _store.EnumerateMessages(streamId, 0, int.MaxValue))
            {
                stream.Events.AddRange(record.Items);
                stream.StreamVersion = record.StreamVersion;
            }
            return stream;
        }
    }

    public sealed class EventStream
    {
        public long StreamVersion;
        public List<object> Events = new List<object>();
    }


    /// <summary>
    /// Is thrown by event store if there were changes since our last version
    /// </summary>
    [Serializable]
    public class OptimisticConcurrencyException : Exception
    {
        public long ActualVersion { get; private set; }
        public long ExpectedVersion { get; private set; }
        public string Id { get; private set; }
        public IList<object> ActualEvents { get; private set; }

        OptimisticConcurrencyException(string message, long actualVersion, long expectedVersion, string id,
            IList<object> serverEvents)
            : base(message)
        {
            ActualVersion = actualVersion;
            ExpectedVersion = expectedVersion;
            Id = id;
            ActualEvents = serverEvents;
        }

        public static OptimisticConcurrencyException Create(long actual, long expected, string streamId,
            IList<object> serverEvents)
        {
            var message = string.Format("Expected v{0} but found v{1} in stream '{2}'", expected, actual, streamId);
            return new OptimisticConcurrencyException(message, actual, expected, streamId, serverEvents);
        }

        protected OptimisticConcurrencyException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
    }


}