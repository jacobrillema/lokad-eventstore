namespace Lokad.EventStore
{
    public struct MessageAttribute
    {
        public readonly string Key;
        public readonly string Value;

        public static readonly MessageAttribute[] Empty = new MessageAttribute[0];

        public MessageAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}