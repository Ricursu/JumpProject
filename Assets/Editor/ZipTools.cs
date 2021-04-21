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
        string zipname = HotUpdate.mReleaseVersion + "." + HotUpdate.mMajorVersion;
        //获取path下的所有文件
        string path = "D://Apk/version/v"+zipname+"/Different";
        string[] files = Directory.GetDirectories(path);
        Debug.LogError(path);
        foreach (string file in files)
        {
            string tempName = file.Replace(path + "\\", "");
            CreateZipStream(path, tempName, file);
        }
        
    }

    /// <summary>
    /// 创建压缩文件输出流
    /// </summary>
    public static void CreateZipStream(string path, string zipname, string filePath)
    {
        if (File.Exists(path + "/" + zipname + ".zip"))
            File.Delete(path + "/" + zipname + ".zip");
        ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(path + "/" + zipname + ".zip"));   //" + zipname + "
        zipOutputStream.SetLevel(6);

        //CreateVersionFile(versionFileName, "version:" + zipname);

        //将筛选后文件件写入压缩的输出流中
        string[] files = Directory.GetFiles(filePath);
        foreach (string name in files)
        {
            bool result = CreateZipFile(name, zipOutputStream);
            if (result == false)
                return;
        }
        //完成压缩
        zipOutputStream.Finish();
        zipOutputStream.Close();
    }

    /// <summary>
    /// 创建压缩文件中的每个小文件，将每个小文件写入压缩的输出流中
    /// </summary>
    /// <param name="versionFileName">版本文件的文件名，压缩文件的过程中需要计算每个小文件的CRC码与MD5码</param>
    /// <param name="filename">需要压缩的带有绝对路径的文件名</param>
    /// <param name="zipOutputStream">写入压缩文件的输出流</param>
    /// <returns></returns>
    static bool CreateZipFile(string filename, ZipOutputStream zipOutputStream)
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

            //FileUtils.WriteFileAppend(versionFileName, Convert.ToString(Path.GetFileName(filename) + " - CRC:" + Crc32.ComputeChecksum(buffer)));
            //FileUtils.WriteFileAppend(versionFileName, Path.GetFileName(filename) + " - MD5:" + EncryptProvider.Md5(Encoding.UTF8.GetString(buffer)));

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
    public static void UnZip()
    {
        Debug.Log("UnZip");
        string version = HotUpdate.mReleaseVersion + "." + HotUpdate.mMajorVersion;
        UnZipApk("D://Apk/" + version + ".apk", "v" + version);
        string newPath = "D://Apk/version/v" + version + "/Source";
        for (int i = HotUpdate.mMajorVersion - 1; i >=0; i--)
        {
            string tempVers = HotUpdate.mReleaseVersion + "." + i;
            string oldPath = "D://Apk/version/v" + tempVers + "/Source";
            string diffPath = "D://Apk/version/v" + version + "/Different/v" + tempVers + "-v" + version;
            DiffTools.buildFileDiff(oldPath, newPath, diffPath);
        }
    }

    static void UnZipApk(string path, string version)
    {
        ZipInputStream zipInput = new ZipInputStream(File.OpenRead(path));
        ZipEntry entry;
        try
        {
            string unzipPath = "D://Apk/version/";
            while ((entry = zipInput.GetNextEntry()) != null)
            {
                byte[] buffer = new byte[entry.Size + 1];
                string directoryName = Path.GetDirectoryName(entry.Name);
                string fileName = Path.GetFileName(entry.Name);

                zipInput.Read(buffer, (int)entry.Offset, (int)entry.Size);

                path = unzipPath;
                path = Path.Combine(path, version + "/Source");
                if (!directoryName.Contains("Lua"))
                    continue;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = Path.Combine(path, fileName);

                buffer = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(buffer));
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                fs.Write(buffer, 0, buffer.Length - 1);
                fs.Flush();
                fs.Dispose();
                fs.Close();
                path = unzipPath;
                path = Path.Combine(path, version + "/Different");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (HotUpdate.mMajorVersion == 0)
                    continue;
            }
        }catch(Exception e)
        {
            Debug.Log(e.GetType() + " " + e.GetBaseException());
        }
        finally
        {
            zipInput.Close();
        }
    }

    [MenuItem("Zip/Log String")]
    public static void LogString()
    {
        string str = Encoding.UTF8.GetString(WebUtils.GetByteFromServer("version.txt"));
        Debug.Log(str);
        string[] strInfo = str.Split('=');
        foreach (string s in strInfo)
            Debug.Log(s);
        //Debug.Log(WebUtils.IsExistFileInServer("http://192.168.3.30/1.1.zip"));


        //string str = "version = 1.1";
        //string[] strInfo = str.Split('=');
        //foreach (string s in strInfo)
        //    Debug.Log(s.Trim());
    }

    [MenuItem("Zip/Build Version File")]
    public  static void BuildVersionFile()
    {
        string version = HotUpdate.mReleaseVersion + "." + HotUpdate.mMajorVersion;
        string path = "D://Apk/version";
        path = Path.Combine(path, "v" + version, "Different");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        FileUtils.CreateFile(path + "/version.txt", Encoding.UTF8.GetBytes("version = " + version));
    }

    [MenuItem("Zip/Get IP Address")]
    public static void GetIPAddress()
    {
        Debug.Log(WebUtils.GetIPAddress());
    }
}
