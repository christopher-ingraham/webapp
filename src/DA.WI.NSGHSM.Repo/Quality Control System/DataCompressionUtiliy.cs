using System.Linq;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace DA.WI.NSGHSM.Repo.QualityControlSystem
{
    class DataCompressionUtility
    {

        public static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (InflaterInputStream decompressionStream = new InflaterInputStream(input))
            {
                decompressionStream.CopyTo(output);
                // Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
            }

            output.Position = 0;
            return output.ToArray();
        }
    }
}
