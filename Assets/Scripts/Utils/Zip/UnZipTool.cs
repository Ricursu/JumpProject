using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System;
using System.Text;

public class UnZipTool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

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

        string path = Application.persistentDataPath + "/Android/Lua/" + zipname;    // "/Android/Lua/version.zip"; // "D://Zip/version.zip";
        string deletefile = path;
        ZipInputStream zipInput = new ZipInputStream(File.OpenRead(path));
        ZipEntry entry;

        Debug.Log(zipname.Substring(0, zipname.LastIndexOf('.')));
        path = Application.persistentDataPath + "/Android/Lua/" + zipname.Substring(0, zipname.LastIndexOf('.')) + "/"; // "D://Zip/version/";
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        Directory.CreateDirectory(path);

        while ((entry = zipInput.GetNextEntry()) != null)
        {
            byte[] buffer = new byte[entry.Size + 1];
            Debug.Log(entry.Name);
            zipInput.Read(buffer, (int)entry.Offset, (int)entry.Size);

            //将解压的文件写到磁盘中
            FileUtils.CreateFile(path + entry.Name, buffer);
        }
        Debug.Log("========================== \n  DELETEFILE DELETEFILE DELETEFILE DELETEFILE \n==========================\n" + deletefile);
        File.Delete(deletefile);
    }
}
