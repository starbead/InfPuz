using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

public class Base64Zip
{
    public static string Zip(string text)
    {
        if (text == null)
            return null;

        var b = Encoding.UTF8.GetBytes(text);
        var compressedStream = new MemoryStream();
        using (var compressorStream = new GZipStream(compressedStream, CompressionMode.Compress, true))
            compressorStream.Write(b, 0, b.Length);

        compressedStream.Position = 0;

        var compressedBytes = new byte[compressedStream.Length];
        compressedStream.Read(compressedBytes, 0, compressedBytes.Length);

        compressedStream.Dispose();
        return Convert.ToBase64String(compressedBytes);
    }

    public static string UnZip(string value)
    {
        byte[] decompressedBytes;

        var compressedStream = new MemoryStream(Convert.FromBase64String(value));
        using (var decompressorStream = new GZipStream(compressedStream, CompressionMode.Decompress))
        {
            using (var decompressedStream = new MemoryStream())
            {
                decompressorStream.CopyTo(decompressedStream);
                decompressedBytes = decompressedStream.ToArray();
            }
        }
        compressedStream.Dispose();
        return Encoding.UTF8.GetString(decompressedBytes);
    }

    public static string MD5Hash(string input)
    {
        var hash = new StringBuilder();
        var md5provider = new MD5CryptoServiceProvider();
        byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        for (int i = 0; i < bytes.Length; i++)
            hash.Append(bytes[i].ToString("x2"));
        return hash.ToString();
    }
}
