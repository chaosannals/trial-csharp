using System.Text;

namespace FastCGI {
    /// <summary>
    /// 请求开始信息内容
    /// </summary>
    public class FastCGIBeginRequestBody
    {
        public byte ReservedB4;
        public byte ReservedB3;
        public byte ReservedB2;
        public byte ReservedB1;
        public byte ReservedB0;

        public FastCGIRole Role { get; private set; }
        public FastCGIFlag Flags { get; private set; }

        public FastCGIBeginRequestBody(byte[] data)
        {
            Role = (FastCGIRole)((data[0] << 8) | data[1]);
            Flags = (FastCGIFlag)data[2];
            ReservedB4 = data[3];
            ReservedB3 = data[4];
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
            builder.Append("Role: ");
            builder.Append(Role.ToString());
            builder.Append(" Flags: ");
            builder.Append(Flags.ToString());
            return builder.ToString();
        }
    }
}