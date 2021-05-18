using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NUnit.Framework;
using System;
using NETCore.Encrypt;
using NullFX.CRC;
/// <summary>
/// 这个类把lua需要用到的所有方法都集合在这里   由Util工具类调用    lua直接在util调用响应方法即可
/// </summary>
public class InterfaceExtendedToLua
{



    /// <summary>
    /// 亚索文件
    /// </summary>
    /// <param name="bytes">要压缩文件的字节数组</param>
    /// <param name="path">压缩后的文件存储路径 完全路径,例如:    D:/Eclipse/configuration/zzzz.gz  </param>
    //注意!!!!!!!   gzip的文件格式以.gz结尾
    public static void CompressFile(byte[] bytes, string path)
    {
        string str = System.Text.Encoding.Default.GetString(bytes);
        string filePath = path;
        byte[] compressedBytes = TestGZip.WriteGzip(str);
        File.WriteAllBytes(filePath, compressedBytes);
    }


    /// <summary>
    /// 解压文件并返回解压后的文件字节数组
    /// </summary>
    /// <param name="path">要解压的文件完全路径  例如:  D:/Eclipse/configuration/zzzz.gz   </param>
    /// 此方法返回解压后的文件字节数组
    /// <returns></returns>

    public static byte[] DecompressFile(string path)
    {

        string tempFilePath = System.IO.Path.GetDirectoryName(path);
        if (tempFilePath != "")//如果解析出来的目录路径不为空
        {
            if (!Directory.Exists(tempFilePath))//判断是否存在
            {
                try
                {
                    Directory.CreateDirectory(path);//创建新路径
                }
                catch (Exception e)
                {
                    Debug.LogError($"路径格式错误{e.Message}");
                    return null;
                }
            }
        }
        else
        {
            Debug.LogError("路径格式错误");
            return null;
        }



        string str = TestGZip.ReadGzipFromFile(path);
        byte[] bytes = System.Text.Encoding.Default.GetBytes(str);

        return bytes;
    }

    /// <summary>
    /// 计算文件字节的MD5值并返回
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string GetMD5(byte[] bytes)
    {
        string str = System.Text.Encoding.Default.GetString(bytes);
        string MD5str = EncryptProvider.Md5(str);
        return MD5str;
    }

    //以下顺序对应传入参数1-6

    /// <summary>
    ///  Standard, 
    ///  Performs CRC 16 using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0
    /// </summary>

    /// <summary>
    ///  Ccitt, 
    ///  A CRC 16 CCITT Utility using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0
    /// </summary>

    /// <summary>
    /// CcittKermit,
    /// Performs CRC 16 CCITT using a reversed x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0
    /// </summary>

    /// <summary>
    /// CcittInitialValue0xFFFF,
    /// Performs CRC 16 CCITT using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0xffff
    /// </summary>

    /// <summary>
    /// CcittInitialValue0x1D0F,
    /// Performs CRC 16 CCITT using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0x1D0F
    /// </summary>

    /// <summary>
    /// Dnp
    /// Performs CRC 16 Distributed Network Protocol (DNP) using reversed x^16 + x^13 + x^12 + x^11 + x^10 + x^8 + x^6 + x^5 + x^2 + 1 (0xA6BC) with an initial CRC value of 0
    /// </summary>


    public static ushort GetCRC16(byte[] bytes, int number = 1)
    {
        switch (number)
        {
            case 1:
                return Crc16.ComputeChecksum(Crc16Algorithm.Standard, bytes);
            case 2:
                return Crc16.ComputeChecksum(Crc16Algorithm.Ccitt, bytes);
            case 3:
                return Crc16.ComputeChecksum(Crc16Algorithm.CcittKermit, bytes);
            case 4:
                return Crc16.ComputeChecksum(Crc16Algorithm.CcittInitialValue0xFFFF, bytes);
            case 5:
                return Crc16.ComputeChecksum(Crc16Algorithm.CcittInitialValue0x1D0F, bytes);
            case 6:
                return Crc16.ComputeChecksum(Crc16Algorithm.Dnp, bytes);
            default:
                return Crc16.ComputeChecksum(Crc16Algorithm.Standard, bytes);
        }
    }









}
