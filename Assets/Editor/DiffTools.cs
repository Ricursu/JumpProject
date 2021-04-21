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

public class DiffTools
{
    [MenuItem("VCDiff/Build Diff")]
    public static void DoEncode()
    {
        Debug.Log(Application.dataPath+"/APK");
        //string[] file = Directory.GetFiles("D://APK");
        //foreach (string apkname in file)
        //    Debug.Log(apkname);

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

    [MenuItem("VCDiff/Build File Diff")]
    public static void DoFileEncode()
    {
        string oldApkPath = "D://APK/old/assets/Lua/";
        string newApkPath = "D://APK/new/assets/Lua/";
        string[] oldFile = Directory.GetFiles(oldApkPath);
        string[] newFile = Directory.GetFiles(newApkPath);
        string diffPath = "D://Apk/Diff/";
        if (!Directory.Exists(diffPath))
            Directory.CreateDirectory(diffPath);
        FileStream fileList = new FileStream("D://APK/Diff/filelist.txt", FileMode.Append, FileAccess.Write);
        foreach (string name in newFile)
        {
            string filename = Path.GetFileName(name);
            using (FileStream dict = new FileStream(Path.Combine(oldApkPath, filename), FileMode.Open, FileAccess.Read) )
            using(FileStream target = new FileStream(name, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer1 = new byte[dict.Length];
                byte[] buffer2 = new byte[target.Length];

                dict.Read(buffer1, 0, buffer1.Length);
                target.Read(buffer2, 0, buffer2.Length);

                string md5B1 = EncryptProvider.Md5(Encoding.UTF8.GetString(buffer1));
                string md5B2 = EncryptProvider.Md5(Encoding.UTF8.GetString(buffer2));

                if (md5B1 == md5B2)
                    continue;


                filename = Path.GetFileNameWithoutExtension(filename);
                byte[] buffer = Encoding.UTF8.GetBytes(filename + "," + md5B1 + "\n");
                fileList.Write(buffer, 0, buffer.Length);


                FileStream output = new FileStream(Path.Combine(diffPath, md5B1), FileMode.Create, FileAccess.Write);

                VCCoder coder = new VCCoder(dict, target, output);
                VCDiffResult result = coder.Encode(false, true);

                if (result != VCDiffResult.SUCCESS)
                {
                    Debug.Log("差分失败");
                }
                output.Dispose();
                output.Close();
            }
        }
        fileList.Dispose();
        fileList.Close();
    }


    [MenuItem("VCDiff/Build APK")]
    public static void DoDecode()
    {
        string Path = "D://APK";
        using (FileStream output = new FileStream(Path + "/main.lua", FileMode.Create, FileAccess.Write))
        using (FileStream dict = new FileStream(Path + "/old/assets/Lua/Main.lua", FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = new byte[dict.Length];
            dict.Read(buffer, 0, buffer.Length);
            string md5s = EncryptProvider.Md5(Encoding.UTF8.GetString(buffer));
            Debug.LogError(md5s);
            FileStream target = new FileStream(Path + "/Diff/" + md5s, FileMode.Open, FileAccess.Read);

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

    public static void buildFileDiff(string olePath, string newPath, string diffPath)
    {
        string[] newFile = Directory.GetFiles(newPath);
        if (!Directory.Exists(diffPath))
            Directory.CreateDirectory(diffPath);
        FileStream fileList = new FileStream(Path.Combine(diffPath, "filelist.txt"), FileMode.Append, FileAccess.Write);
        foreach (string name in newFile)
        {
            string filename = Path.GetFileName(name);
            using (FileStream dict = new FileStream(Path.Combine(olePath, filename), FileMode.Open, FileAccess.Read))
            using (FileStream target = new FileStream(name, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer1 = new byte[dict.Length];
                byte[] buffer2 = new byte[target.Length];

                dict.Read(buffer1, 0, buffer1.Length);
                target.Read(buffer2, 0, buffer2.Length);

                string md5B1 = EncryptProvider.Md5(Encoding.UTF8.GetString(buffer1));
                string md5B2 = EncryptProvider.Md5(Encoding.UTF8.GetString(buffer2));

                if (md5B1 == md5B2)
                    continue;


                filename = Path.GetFileNameWithoutExtension(filename);
                byte[] buffer = Encoding.UTF8.GetBytes(filename + "," + md5B1 + "\n");
                fileList.Write(buffer, 0, buffer.Length);


                FileStream output = new FileStream(Path.Combine(diffPath, md5B1), FileMode.Create, FileAccess.Write);

                VCCoder coder = new VCCoder(dict, target, output);
                VCDiffResult result = coder.Encode(false, true);

                if (result != VCDiffResult.SUCCESS)
                {
                    Debug.Log("差分失败");
                }
                output.Dispose();
                output.Close();
            }
        }
        fileList.Dispose();
        fileList.Close();

    }

}
