using System.Text;

namespace FastCGI
{
    /// <summary>
    /// 请求结束内容
    /// </summary>
    public class FastCGIEndRequestBody
    {
        public byte ReservedB2;
        public byte ReservedB1;
        public byte ReservedB0;

        public int AppStatus { get; private set; }
        public FastCGIProtocolStatus ProtocolStatus { get; private set; }

        public FastCGIEndRequestBody(byte[] data)
        {
            AppStatus = (data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3];
            ProtocolStatus = (FastCGIProtocolStatus)data[4];
            ReservedB2 = data[5];
            ReservedB1 = data[6];
            ReservedB0 = data[7];
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("AppStatus: ");
            builder.Append(AppStatus.ToString());
            builder.Append(" ProtocolStatus: ");
            builder.Append(ProtocolStatus.ToString());
            return builder.ToString();
        }
    }
}