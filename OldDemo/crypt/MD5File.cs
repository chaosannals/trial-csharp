using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Crypt
{
    public static class MD5File
    {
        public static string Make(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] r = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < r.Length; i++)
                {
                    sb.Append(r[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}