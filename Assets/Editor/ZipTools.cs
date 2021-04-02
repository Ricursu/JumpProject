using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Text;
using CRC;
using NETCore.Encrypt;

public class ZipTools
{
    /// <summary>
    /// unity的菜单栏下的zip/Build zip 压缩StreamingAsset下的所有打包好的asset bundle文件
    /// </summary>
    [MenuItem("Zip/Build Zip")]
    static void ZipStreamingAsset()
    {
        string zipname = HotUpdate.mReleaseVersion + "." + HotUpdate.mMajorVersion + "." + HotUpdate.mMinorVersion;
        //获取path下的所有文件
        string path = "D://Apk/Diff";
        string[] files = Directory.GetFiles(path);
        List<string> fileList = new List<string>();
        //筛选需要压缩的文件
        //foreach (string name in files)
        //{
        //    if (!name.EndsWith(".meta") && Path.GetFileName(name).Contains("unity3d"))
        //        fileList.Add(name);
        //}
        //创建压缩文件输出流
        if (File.Exists(path + "/version" + zipname + ".zip"))
            File.Delete(path + "/version" + zipname + ".zip");
        ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(path + "/version.zip"));   //" + zipname + "
        zipOutputStream.SetLevel(6);

        //创建压缩文件版本文件
        string versionFileName = "version.txt";
        versionFileName = path + "/" + versionFileName;
        if (File.Exists(versionFileName))
            File.Delete(versionFileName);

        FileUtils.WriteFileAppend(versionFileName, "version:" + zipname);
        //CreateVersionFile(versionFileName, "version:" + zipname);

        //将筛选后文件件写入压缩的输出流中
        foreach (string name in files)
        {
            bool result = CreateZipFile(versionFileName, name, zipOutputStream);
            if (result == false)
                return;
        }
        //完成压缩
        zipOutputStream.Finish();
        zipOutputStream.Close();

        FileUtils.WriteFileAppend(Application.streamingAssetsPath + "/VERSION", "[VERSION" + zipname + "]");
    }

    /// <summary>
    /// 创建压缩文件中的每个小文件，将每个小文件写入压缩的输出流中
    /// </summary>
    /// <param name="versionFileName">版本文件的文件名，压缩文件的过程中需要计算每个小文件的CRC码与MD5码</param>
    /// <param name="filename">需要压缩的带有绝对路径的文件名</param>
    /// <param name="zipOutputStream">写入压缩文件的输出流</param>
    /// <returns></returns>
    static bool CreateZipFile(string versionFileName, string filename, ZipOutputStream zipOutputStream)
    {
        ZipEntry zipEntry = new ZipEntry(Path.GetFileName(filename));
        FileStream fs = null;
        Debug.Log("[ZipCreateZipFile]: " + Path.GetFileName(filename));
        try
        {
            fs = File.OpenRead(filename);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            zipEntry.DateTime = DateTime.Now;
            zipEntry.Size = fs.Length;
            zipOutputStream.PutNextEntry(zipEntry);
            zipOutputStream.Write(buffer, 0, buffer.Length);
            Debug.Log(filename + "   " + Crc32.ComputeChecksum(buffer));

            FileUtils.WriteFileAppend(versionFileName, Convert.ToString(Path.GetFileName(filename) + " - CRC:" + Crc32.ComputeChecksum(buffer)));
            FileUtils.WriteFileAppend(versionFileName, Path.GetFileName(filename) + " - MD5:" + EncryptProvider.Md5(Encoding.UTF8.GetString(buffer)));

            //CreateVersionFile(versionFileName, Convert.ToString(Path.GetFileName(filename) + " - CRC:" + Crc32.ComputeChecksum(buffer)));
            //CreateVersionFile(versionFileName, Path.GetFileName(filename) + " - MD5:" + EncryptProvider.Md5(Encoding.UTF8.GetString(buffer)));
        }
        catch(Exception e)
        {
            Debug.Log(e.GetType() + "" + e.GetBaseException() +  e.StackTrace);
            return false;
        }
        finally
        {
            fs.Close();
            fs.Dispose();
        }
        return true;
    }


    [Obsolete]
    /// <summary>
    /// 这部分需要抽离出来放到文件读写中（已弃用 文件处理调用FileUtils类）
    /// </summary>
    /// <param name="filename">读取的文件的包含文件路径的文件名</param>
    /// <param name="checkString">写入文件的字符串</param>
    public static void CreateVersionFile(string filename, string checkString)
    {
        Byte[] buffer = Encoding.UTF8.GetBytes(checkString + "\n");
        FileStream fs = null;
        if (File.Exists(filename))
        {
            fs = File.Open(filename, FileMode.Append);
        }
        else
        {
            fs = File.Open(filename, FileMode.Create);
        }
        fs.Write(buffer, 0, buffer.Length);
        fs.Close();
        fs.Dispose();
    }

    [Obsolete]
    [MenuItem("Zip/UnZip")]
    static void UnZip()
    {
        Debug.Log("UnZip");
        string path = "D://Apk/new.apk";
        ZipInputStream zipInput = new ZipInputStream(File.OpenRead(path));
        ZipEntry entry;
        while((entry = zipInput.GetNextEntry()) != null)
        {
            byte[] buffer = new byte[entry.Size + 1];
            string directoryName = Path.GetDirectoryName(entry.Name);
            string fileName = Path.GetFileName(entry.Name);

            zipInput.Read(buffer, (int)entry.Offset, (int)entry.Size);

            path = "D://Apk/new/";
            path = Path.Combine(path, directoryName);
            if (!path.Contains("Lua"))
                continue;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, fileName);
            FileInfo fileInfo = new FileInfo(path);
            Debug.Log(path);
            FileStream fs = fileInfo.Create();
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
            fs.Close();
            fs.Dispose();
        }
    }
}
