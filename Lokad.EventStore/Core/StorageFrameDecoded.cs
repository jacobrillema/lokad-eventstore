namespace Lokad.EventStore.Core
{
    public struct StorageFrameDecoded
    {
        public readonly byte[] Bytes;
        public readonly string Name;
        public readonly long Stamp;

        public bool IsEmpty
        {
            get { return Bytes.Length == 0 && Stamp == 0 && string.IsNullOrEmpty(Name); }
        }

        public StorageFrameDecoded(byte[] bytes, string name, long stamp)
        {
            Bytes = bytes;
            Name = name;
            Stamp = stamp;
        }
    }
}