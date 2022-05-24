using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;

namespace DynUpdateService
{
    internal class DynUnpacker
    {
        public string Key { get; private set; }
        public DynUnpacker(string key)
        {
            Key = key;
        }

        public byte[] Unpack()
        {
            var data = Download();
            "data size: {0}".Log(data.Length);
            return Unpack(Key, data);
        }

        public static byte[] Download()
        {
            string url = "http://127.0.0.1:33333/DynTarget.dyn";
            using (WebClient client = new WebClient())
            {
                return client.DownloadData(url);
            }
        }

        public static byte[] Unpack(string key, byte[] data)
        {
            byte[] kb = Encoding.UTF8.GetBytes(key);
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = data.Take(16).ToArray();
            var hash = data.Skip(16).Take(32).ToArray();
            var raw = data.Skip(16 + 32).ToArray();
            using (var hmac = new HMACSHA256(kb))
            {
                byte[] chash = hmac.ComputeHash(raw);
                if (!hash.SequenceEqual(chash))
                {
                    "散列校验错误 {0} {1}".Log(Convert.ToBase64String(hash), Convert.ToBase64String(chash));
                    return null;
                }
            }
            "raw: {0}".Log(raw.Length);

            using (var decryptor = aes.CreateDecryptor())
            {
                using (var m = new MemoryStream(raw))
                {
                    using (var c = new CryptoStream(m, decryptor, CryptoStreamMode.Read))
                    {
                        var r = new byte[raw.Length];
                        var rc = c.Read(r, 0, raw.Length);
                        return r.Take(rc).ToArray();
                    }
                }
            }
        }
    }
}
