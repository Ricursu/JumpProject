using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System;
using System.Text;

public class UnZipTool : MonoBehaviour
{
    /// <summary>
    /// 解压文件名为zipname的压缩文件
    /// </summary>
    /// <param name="zipname">需要解压的压缩文件</param>
    /// <returns></returns>
    public static bool UnZipPackage(string zipname)
    {
        try
        {
            UnZip(zipname);
        }catch(Exception e)
        {
            Debug.Log("========================== \n  UNZIP ERROR  \n==========================");
            Debug.Log("===========================\n" + e.GetType() + e.GetBaseException() + "\n================================================");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 解压文件名为zipname的压缩文件
    /// </summary>
    /// <param name="zipname">需要解压的压缩文件</param>
    /// <returns></returns>
    public static void UnZip(string zipname)
    {
        Debug.Log("========================== \n  UNZIP UNZIP UNZIP UNZIP  \n==========================");

        string path = Path.Combine(Application.persistentDataPath, zipname);    // "/Android/Lua/version.zip"; // "D://Zip/version.zip";
        string deletefile = path;
        ZipInputStream zipInput = new ZipInputStream(File.OpenRead(path));
        ZipEntry entry;

        Debug.Log(zipname.Substring(0, zipname.LastIndexOf('.')));
        path = path.Replace(".zip", ""); // "D://Zip/version/";
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        Directory.CreateDirectory(path);

        while ((entry = zipInput.GetNextEntry()) != null)
        {
            byte[] buffer = new byte[entry.Size + 1];
            Debug.Log(entry.Name);
            zipInput.Read(buffer, (int)entry.Offset, (int)entry.Size);

            //将解压的文件写到磁盘中
            FileUtils.CreateFile(Path.Combine(path, entry.Name), buffer);
        }
        Debug.Log("========================== \n  DELETEFILE DELETEFILE DELETEFILE DELETEFILE \n==========================\n" + deletefile);
        File.Delete(deletefile);
    }

    public static void UnZipApk(string path)
    {
        try
        {
            Debug.Log(path);
            ZipInputStream zipInput = new ZipInputStream(File.OpenRead(path));
            ZipEntry entry;
            while ((entry = zipInput.GetNextEntry()) != null)
            {
                byte[] buffer = new byte[entry.Size + 1];
                string directoryName = Path.GetDirectoryName(entry.Name);
                string fileName = Path.GetFileName(entry.Name);

                zipInput.Read(buffer, (int)entry.Offset, (int)entry.Size);

                path = Application.persistentDataPath;
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
        catch(Exception e)
        {
            Debug.LogError(e.GetType() + "" + e.GetBaseException());
        }
    }
}
