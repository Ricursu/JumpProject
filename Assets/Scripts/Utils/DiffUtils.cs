using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VCDiff.Decoders;
using VCDiff.Includes;

public class DiffUtils
{
    public static void ReductionFile(string luaPath, string listFilePath)
    {
        string[] files = File.ReadAllLines(listFilePath);
        foreach(string fileName in files)
        {
            if (fileName.Length < 5)
                break;
            string[] fileInfo = fileName.Split(',');
            string luaFile = fileInfo[0] + ".lua";
            //Debug.Log("\n===========================\n" + listFilePath + luaFile + "\n" + luaPath + luaFile + "\n" + fileName + "\n===========================\n");
            Debug.LogError("\n==============\n" + fileInfo[0] + fileInfo[1] + "\n==============\n");
            string reductionPath = Path.Combine(Application.persistentDataPath, luaFile);
            string oldFilePath = Path.Combine(luaPath, luaFile);
            string diffFilePath = Path.Combine(Path.GetDirectoryName(listFilePath), fileInfo[1]);
            Reduction(reductionPath, oldFilePath, diffFilePath);
            FileUtils.CopyFileToPath(reductionPath, oldFilePath);
            File.Delete(reductionPath);
        }
    }

    public static bool Reduction(string reductionPath, string oldFilePath, string diffFilePath)
    {
        try
        {
            using (FileStream output = new FileStream(reductionPath, FileMode.CreateNew, FileAccess.Write))             //旧apk包打入差分文件后的新apk包
            using (FileStream dict = new FileStream(oldFilePath, FileMode.Open, FileAccess.Read))                     //旧版apk包
            using (FileStream target = new FileStream(diffFilePath, FileMode.Open, FileAccess.Read))                //差分包
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
