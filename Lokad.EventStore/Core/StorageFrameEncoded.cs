namespace Lokad.EventStore.Core
{
    public struct StorageFrameEncoded
    {
        public readonly byte[] Data;
        public readonly byte[] Hash;

        public StorageFrameEncoded(byte[] data, byte[] hash)
        {
            Data = data;
            Hash = hash;
        }
    }
}