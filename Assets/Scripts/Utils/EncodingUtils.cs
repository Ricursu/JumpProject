using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EncodingUtils
{

    public static string GetString(byte[] buffer)
    {
        return Encoding.UTF8.GetString(buffer);
    }
    public static byte[] GetBytes(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }



}
