using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VCDiff.Decoders;
using VCDiff.Includes;

public class DiffUtils
{
    public static bool ReductionApk(string newApkPath, string oldApkPath, string diffPath)
    {
        string Path = Application.persistentDataPath;
        try
        {
            using (FileStream output = new FileStream(Path + newApkPath, FileMode.Create, FileAccess.Write))             //旧apk包打入差分文件后的新apk包
            using (FileStream dict = new FileStream(Path + oldApkPath, FileMode.Open, FileAccess.Read))                     //旧版apk包
            using (FileStream target = new FileStream(Path + diffPath, FileMode.Open, FileAccess.Read))                //差分包
            {
                VCDecoder decoder = new VCDecoder(dict, target, output);

                VCDiffResult result = decoder.Start();
                if (result != VCDiffResult.SUCCESS)
                {
                    Debug.Log("还原失败");
                    return false;
                }
                long byteWritten = 0;
                result = decoder.Decode(out byteWritten);
                if (result != VCDiffResult.SUCCESS)
                {
                    Debug.Log("还原失败");
                    return false;
                }
                Debug.Log(byteWritten);
            }
        }catch(Exception e)
        {
            Debug.Log(e.GetType() + "" + e.GetBaseException());
            return false;
        }
        return true;
    }
}
