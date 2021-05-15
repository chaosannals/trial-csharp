using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.Json;

namespace Crypt
{
    public static class Aes256
    {
        public static RNGCryptoServiceProvider RandomProvider { get { return new RNGCryptoServiceProvider(); } }

        public static string Encrypt<T>(string key, T data)
        {
            byte[] iv = new byte[16];
            byte[] kb = Encoding.UTF8.GetBytes(key);
            RandomProvider.GetBytes(iv);
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = kb;
            aes.IV = iv;
            using (var encryptor = aes.CreateEncryptor())
            {
                using (var m = new MemoryStream())
                {
                    using (var c = new CryptoStream(m, encryptor, CryptoStreamMode.Write))
                    {
                        using (var w = new StreamWriter(c))
                        {
                            w.Write(JsonSerializer.Serialize<T>(data));
                        }
                        var text = m.ToArray();
                        using (var hmac = new HMACSHA256(kb))
                        {
                            byte[] hash = hmac.ComputeHash(text);
                            List<byte> buffer = new List<byte>();
                            buffer.AddRange(iv);
                            buffer.AddRange(hash);
                            buffer.AddRange(text);
                            return Convert.ToBase64String(buffer.ToArray());
                        }
                    }
                }
            }
        }

        public static T Decrypt<T>(string key, string data) where T : class
        {
            byte[] kb = Encoding.UTF8.GetBytes(key);
            var raw = Convert.FromBase64String(data);
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = raw.Take(16).ToArray();
            var hash = raw.Skip(16).Take(32).ToArray();
            var text = raw.Skip(16 + 32).ToArray();
            using (var hmac = new HMACSHA256(kb))
            {
                byte[] chash = hmac.ComputeHash(text);
                for (int i = 0; i < 32; ++i)
                {
                    if (hash[i] != chash[i])
                    {
                        return null;
                    }
                }
            }
            using (var decryptor = aes.CreateDecryptor())
            {
                using (var m = new MemoryStream(text))
                {
                    using (var c = new CryptoStream(m, decryptor, CryptoStreamMode.Read))
                    {
                        using (var r = new StreamReader(c))
                        {
                            var s = r.ReadToEnd();
                            return JsonSerializer.Deserialize<T>(s);
                        }
                    }
                }
            }
        }
    }
}
