using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VCDiff.Encoders;
using VCDiff.Shared;
using VCDiff.Includes;
using UnityEditor;
using VCDiff.Decoders;
using NETCore.Encrypt;
using System.Text;

public class DiffUtils
{
    [MenuItem("VCDiff/Build Diff")]
    public static void DoEncode()
    {
        Debug.Log(Application.dataPath+"/APK");
        string[] file = Directory.GetFiles("D://APK");
        foreach (string apkname in file)
            Debug.Log(apkname);

        string Path = "D://APK";
        using (FileStream dict = new FileStream(Path + "/old.apk", FileMode.Open, FileAccess.Read))
        using (FileStream target = new FileStream(Path + "/new.apk", FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = new byte[dict.Length];
            dict.Read(buffer, 0, (int)dict.Length);
            string md5 = Encoding.UTF8.GetString(buffer);
            md5 = EncryptProvider.Md5(md5);

            FileStream output = new FileStream(Path + "/" + md5, FileMode.Create, FileAccess.Write);

            VCCoder coder = new VCCoder(dict, target, output);
            VCDiffResult result = coder.Encode();
            if (result != VCDiffResult.SUCCESS)
            {
                Debug.Log("差分失败");
            }

            output.Close();
            output.Dispose();
        }
    }

    [MenuItem("VCDiff/Build APK")]
    public static void DoDecode()
    {
        string Path = "D://APK";
        using (FileStream output = new FileStream(Path + "/newapk.apk", FileMode.Create, FileAccess.Write))
        using (FileStream dict = new FileStream(Path + "/old.apk", FileMode.Open, FileAccess.Read))
        using (FileStream target = new FileStream(Path + "/patch.diff", FileMode.Open, FileAccess.Read))
        {
            VCDecoder decoder = new VCDecoder(dict, target, output);

            VCDiffResult result = decoder.Start();
            if(result != VCDiffResult.SUCCESS)
            {
                Debug.Log("还原失败");
            }

            long byteWritten = 0;
            result = decoder.Decode(out byteWritten);
            if (result != VCDiffResult.SUCCESS)
            {
                Debug.Log("还原失败");
            }

            Debug.Log(byteWritten);

        }
    }

    [MenuItem("VCDiff/General MD5")]
    public static void GeneralMD5()
    {
        string Path = "D://APK";
        using (FileStream output = new FileStream(Path + "/new.apk", FileMode.Open, FileAccess.Read))
        using (FileStream dict = new FileStream(Path + "/old.apk", FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = new byte[output.Length];
            output.Read(buffer, 0, buffer.Length);
            Debug.Log( EncryptProvider.Md5(Encoding.UTF8.GetString(buffer)) );
            buffer = new byte[dict.Length];
            dict.Read(buffer, 0, buffer.Length);
            Debug.Log( EncryptProvider.Md5(Encoding.UTF8.GetString(buffer)) );


        }
    }
}
