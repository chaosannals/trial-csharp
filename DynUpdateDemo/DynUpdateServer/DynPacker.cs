using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.IO;
using ILMerging;
using log4net;

namespace DynUpdateServer
{
    internal class DynPacker
    {
        public static RNGCryptoServiceProvider RandomProvider = new RNGCryptoServiceProvider();

        public string Key { get; private set; }
        public List<string> Directories { get; private set; }

        private ILog log = null;

        public DynPacker(string key)
        {
            log = LogManager.GetLogger(typeof(DynPacker));
            Key = key;
            Directories = new List<string>();
        }

        public byte[] Pack(string rdir, string webroot)
        {
            string target = Path.Combine(webroot, "DynTarget.dll");
            string packpath = Path.Combine(webroot, "DynTarget.dyn");
            byte[] data = Merge(rdir, target);
            byte[] result = Pack(Key, data);
            log.InfoFormat("data size: {0} result size {1}", data.Length, result.Length);
            File.WriteAllBytes(packpath, result);
            return result;
        }

        public static byte[] Merge(string rdir, string target)
        {
            string[] rfiles = Directory.GetFiles(rdir, "*.dll");
            rfiles = rfiles.OrderBy(i => i.Length).ToArray();

            ILMerge m = new ILMerge();
            m.Log = true;
            m.LogFile = "merge.log";
            m.AllowZeroPeKind = false;
            m.CopyAttributes = true;
            m.DebugInfo = false;
            m.TargetKind = ILMerge.Kind.Dll;
            m.OutputFile = target;
            m.SetTargetPlatform("v2", "");

            m.SetInputAssemblies(rfiles);
            m.SetSearchDirectories(new string[] {
                rdir,
                // @"G:\trcs\DynUpdateDemo\packages\Newtonsoft.Json.13.0.1\lib\net35",
                // @"G:\trcs\DynUpdateDemo\packages\log4net.2.0.14\lib\net35",
            });
            m.Merge();

            return File.ReadAllBytes(target);
        }

        public byte[] Pack(string key, byte[] data)
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
                        using (var w = new BinaryWriter(c))
                        {
                            w.Write(data);
                        }
                        var raw = m.ToArray();
                        using (var hmac = new HMACSHA256(kb))
                        {
                            byte[] hash = hmac.ComputeHash(raw);
                            List<byte> buffer = new List<byte>();
                            log.InfoFormat("iv: {0} hash: {1} text: {2}", iv.Length, hash.Length, raw.Length);
                            buffer.AddRange(iv);
                            buffer.AddRange(hash);
                            buffer.AddRange(raw);
                            return buffer.ToArray();
                        }
                    }
                }
            }
        }
    }
}
