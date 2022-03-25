using System;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace Spider
{
    class ZipFile
    {
        public static void Decompass(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            using (FileStream ofs = fi.OpenRead())
            {
                string fn = fi.FullName;
                string target = fn.Remove(fn.Length - fi.Extension.Length);

                using (GZipStream gzip = new GZipStream(ofs, CompressionMode.Decompress, true))
                {
                    byte[] checkLengthBuffer = new byte[4];
                    gzip.Position = gzip.Length - 4;
                    gzip.Read(checkLengthBuffer, 0, 4);
                    gzip.Position = 0;
                    int checkLength = BitConverter.ToInt32(checkLengthBuffer, 0);
                    byte[] buffer = new byte[checkLength + 100];
                    int offset = 0;
                    int total = 0;
                    while (true)
                    {
                        int rc = gzip.Read(buffer, offset, 100);
                        if (rc == 0) break;
                        offset += rc;
                        total += rc;
                    }
                    using (FileStream tfs = new FileStream(target, FileMode.Create))
                    {
                        tfs.Write(buffer, 0, total);
                        tfs.Flush();
                    }
                }
            }
        }
    }
}