namespace FastCGI
{
    /// <summary>
    /// 请求头
    /// </summary>
    public class FastCGIHeader
    {
        public byte Version { get; private set; }
        public FastCGIType Type { get; private set; }
        public int RequestId { get; private set; }
        public int ContentLength { get; private set; }
        public byte PaddingLength { get; private set; }
        public byte Reserved { get; private set; }

        public int MessageLength { get; private set; }

        public FastCGIHeader(byte[] data)
        {
            Version = data[0];
            Type = (FastCGIType)data[1];
            RequestId = (data[2] << 8) | data[3];
            ContentLength = (data[4] << 8) | data[5];
            PaddingLength = data[6];
            Reserved = data[7];
            MessageLength = 8 + ContentLength + PaddingLength;
        }
    }
}