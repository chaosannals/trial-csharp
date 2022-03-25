using System.Linq;
using System.Text;

namespace FastCGI {
    /// <summary>
    /// 请求信息
    /// </summary>
    public class FastCGIMessage
    {
        public FastCGIHeader Header;
        public byte[] Body;

        public FastCGIMessage(FastCGIHeader header, byte[] body)
        {
            Header = header;
            Body = body;
        }

        public FastCGIBeginRequestBody AsBeginBody()
        {
            return new FastCGIBeginRequestBody(Body);
        }

        public FastCGIEndRequestBody AsEndBody()
        {
            return new FastCGIEndRequestBody(Body);
        }

        public string AsUTF8Body()
        {
            return Encoding.UTF8.GetString(Body.ToArray());
        }

        /// <summary>
        /// 格式话出基本信息。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ID: ");
            builder.Append(Header.RequestId.ToString());
            builder.Append(" Length: [");
            builder.Append(Header.MessageLength.ToString());
            builder.Append(" / (8+");
            builder.Append(Header.ContentLength.ToString());
            builder.Append("+");
            builder.Append(Header.PaddingLength.ToString());
            builder.Append(")] Type: ");
            builder.Append(Header.Type.ToString());
            return builder.ToString();
        }
    }
}