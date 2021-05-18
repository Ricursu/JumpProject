using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Util_xia
{

    public static void DebugProxy(int number)
    {
        Debug.LogError(number.ToString());
    }
    public static void DebugProxy(string mg)
    {
        Debug.LogError(mg);
    }

    public static ushort GetCRC(byte[] bytes, int number = 1)
    {
        return InterfaceExtendedToLua.GetCRC16(bytes, number);
    }
    public static string GetMD5(byte[] bytes)
    {
        return InterfaceExtendedToLua.GetMD5(bytes);
    }

    public static void CompressFile(byte[] bytes, string path)
    {
        InterfaceExtendedToLua.CompressFile(bytes, path);
    }

    public static byte[] DecompressFile(string path)
    {
        return InterfaceExtendedToLua.DecompressFile(path);
    }




}

