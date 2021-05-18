using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

public class TestGZip 
{
    /// <summary>
    /// 将Gzip的byte数组读取为字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string ReadGzip(byte[] bytes, string encoding = "GB2312")
    {
        string result = string.Empty;
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            using (GZipStream decompressedStream = new GZipStream(ms, CompressionMode.Decompress))
            {
                using (StreamReader sr = new StreamReader(decompressedStream, Encoding.GetEncoding(encoding)))
                {
                    result = sr.ReadToEnd();
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 将字符串压缩成Gzip格式的byte数组
    /// </summary>
    /// <param name="str"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static byte[] WriteGzip(string str, string encoding = "GB2312")
    {
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes(str);
        using (MemoryStream ms = new MemoryStream())
        {
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
    }

    /// <summary>
    /// 解压Gzip文件，返回字符串
    /// </summary>
    /// <param name="fileName">文件全路径</param>
    /// <returns>字符串</returns>
    public static string ReadGzipFromFile(string fileName)
    {
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (GZipStream decompressedStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                StreamReader reader = new StreamReader(decompressedStream);
                string result = reader.ReadToEnd();//重点
                reader.Close();
                return result;
            }
        }
    }
}
