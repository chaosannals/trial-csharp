using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HexBin
{
    [Serializable]
    class TestInfo
    {
        public int Id;
        public string Name;
    }

    class Program
    {
        static void Main(string[] args)
        {
            string hex = "AAEEABCDEF0102030405";
            List<byte> bin = new List<byte>();
            for (int i = 0; i < hex.Length; i += 2)
            {
                string v = hex.Substring(i, 2);
                bin.Add(Convert.ToByte(v, 16));
            }
            using var f = new FileStream("bin", FileMode.OpenOrCreate, FileAccess.Write);
            f.Write(bin.ToArray());
        }

        static void BinFormat()
        {
            var ti = new TestInfo
            {
                Id = 123,
                Name = "Test Info",
            };
            BinaryFormatter formatter = new BinaryFormatter();
            using (var w = new FileStream("bin", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(w, ti);
            }

            using var r = new FileStream("bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            var rti = formatter.Deserialize(r) as TestInfo;
            Console.WriteLine(rti.Name);
        }
    }
}
