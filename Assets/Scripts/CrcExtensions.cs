using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrcExtensions
{
    /// <summary>
    /// 校验和使用的一组常用操作方法
    /// </summary>
    public static ushort ByteSwap(this ushort value)
    {
        return (ushort)(((value & 0xff00) >> 8) | (value & 0x00ff) << 8);
    }

    public static ushort ByteSwapCompliment(this ushort value)
    {
        return ByteSwap((ushort)(~value));
    }
}
