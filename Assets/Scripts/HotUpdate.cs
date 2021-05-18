//using NETCore.Encrypt;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using UnityEngine;
//using UnityEngine.Networking;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HotUpdate
{
    // Start is called before the first frame update
    public static int mReleaseVersion = 1;
    public static int mMajorVersion = 0;

    public static Slider objProcessBar;
    public static Text Loadingprogress;
    public static Text Loadingimformation;


    public HotUpdate()
    {
        Debug.Log("HotUpdate Class " + mReleaseVersion + "." + mMajorVersion);
        objProcessBar = GameObject.Find("Slider").GetComponent<Slider>();
        Loadingprogress = GameObject.Find("Loadingprogress").GetComponent<Text>();
        Loadingimformation = GameObject.Find("Loadingimformation").GetComponent<Text>();
    }

    public static void ChangeLoadingprogress(string s)
    {
        Loadingprogress.text = "Loading progress   " + s.ToString() + "%";
    }
    public static void ChangeLoadingimformation(string s)
    {
        Loadingimformation.text += s.ToString() + "\n";
    }
    public static void ChangeSlider(string s)
    {
        double process = System.Convert.ToDouble(s);
        objProcessBar.value = (float)process;
    }
    public static void ClearSlider()
    {
        WebUtils.processSlider = (0).ToString();
        WebUtils.processText = (0).ToString();
        new System.Threading.Thread(delegate () { WebUtils.isDone = false; System.Threading.Thread.Sleep(1000); WebUtils.isDone = true; }).Start();
        WebUtils.processSlider = (1).ToString();
        WebUtils.processText = (100).ToString();
    }
    public static void FullSlider()
    {
        //new System.Threading.Thread(delegate () { WebUtils.isDone = false; System.Threading.Thread.Sleep(500); WebUtils.isDone = true; }).Start();
    }

    //    /// <summary>
    //    /// 检查更新
    //    /// </summary>
    //    //void Awake()
    //    //{

    //    //    return;

    //    //    //string packageName = "com." + Application.companyName + "." + Application.productName;
    //    //1、创建版本文件config.ini
    //    CreateVersionFile();
    //    //2、判断是否需要更新
    //    if (!IsUpdate())
    //        return;

    //    //3、判断是否需要释放APK
    //    if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Lua")))
    //    {
    //        //3.1、从文件管理器中获取APK
    //        GetApkFromFileManager();
    //        //3.2、 解压APK
    //        UnZipTool.UnZipApk(Application.persistentDataPath + "/base.apk", Path.Combine(Application.persistentDataPath, "Lua"));
    //    }

    //    //4、获取服务器压缩文件
    //    GetZipFromServer();
    //    //5、解压第4步中获取的压缩文件
    //    UnZipFile();
    //    //6、还原压缩文件中的差分包或将压缩文件中的Lua文件复制到本地文件
    //    RedutionFile();

    //    //7、 更新本地版本文件version.ini的内容
    //    FileUtils.CreateFile(Path.Combine(Application.persistentDataPath, "config.ini"), WebUtils.GetByteFromServer("version.txt"));
    //    //}

    //    private void RedutionFile()
    //    {
    //        string[] files = Directory.GetFiles(Path.Combine(Application.persistentDataPath, "Temp"));
    //        foreach (string file in files)
    //        {
    //            if (file.EndsWith(".lua"))
    //            {
    //                FileUtils.CopyFileToPath(file, file.Replace("Temp", "Lua"));
    //            }
    //        }
    //        DiffUtils.ReductionFile(Path.Combine(Application.persistentDataPath, "Lua"), Path.Combine(Application.persistentDataPath, "Temp/filelist.txt"));
    //    }

    //    private void UnZipFile()
    //    {
    //        string version = Encoding.UTF8.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    //        string[] value = version.Split('=');
    //        value[1] = value[1].Trim();
    //        string serverVersion = Encoding.UTF8.GetString(WebUtils.GetByteFromServer("version.txt"));
    //        string[] serverValue = serverVersion.Split('=');
    //        serverValue[1] = serverValue[1].Trim();

    //        string filename = "v" + value[1] + "-v" + serverValue[1] + ".zip";
    //        Debug.LogError("\n=========\n" + filename + "\n=========\n");
    //        UnZipTool.UnZip(filename, Path.Combine(Application.persistentDataPath, "Temp"));
    //    }

    //    private void GetZipFromServer()
    //    {
    //        string version = Encoding.UTF8.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    //        string[] value = version.Split('=');
    //        value[1] = value[1].Trim();
    //        string serverVersion = Encoding.UTF8.GetString(WebUtils.GetByteFromServer("version.txt"));
    //        string[] serverValue = serverVersion.Split('=');
    //        serverValue[1] = serverValue[1].Trim();

    //        string filename = "v" + value[1] + "-v" + serverValue[1] + ".zip";
    //        WebUtils.GetFileFromServer(filename);
    //    }

    //    /// <summary>
    //    /// 判断是否需要更新
    //    /// 通过获取服务器版本文件和本地版本文件进行比较
    //    /// </summary>
    //    /// <returns></returns>
    //    private bool IsUpdate()
    //    {
    //        if (Directory.Exists(Application.persistentDataPath + "/Temp"))
    //            Directory.Delete(Application.persistentDataPath + "/Temp", true);
    //        string serverVersion = Encoding.UTF8.GetString(WebUtils.GetByteFromServer("version.txt"));
    //        string localVersion = Encoding.UTF8.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    //        string[] serverInfo = serverVersion.Split('=');
    //        string[] localInfo = localVersion.Split('=');
    //        if (serverInfo[1].Trim() == localInfo[1].Trim())
    //            return false;
    //        return true;
    //    }

    //    /// <summary>
    //    /// 通过UnityWebRequest从服务器端下载文件名为filename的文件
    //    /// </summary>
    //    /// <param name="fileName">需要下载的文件名</param>
    //    /// <returns></returns>
    //    void ReadAssetBundle(string fileName)
    //    {
    //        string url = "http://10.230.17.74/" + fileName;
    //        UnityWebRequest request = UnityWebRequest.Get(url);
    //        request.SendWebRequest();
    //        while (!request.isDone)
    //        {
    //            Debug.Log("ReadAssetBundle:\n====================\n" + request.downloadProgress + "\n====================");

    //        }

    //        if (request.isDone)                  //下载完成
    //        {
    //            byte[] bytes = request.downloadHandler.data;

    //            if (Application.platform == RuntimePlatform.Android)
    //            {
    //                string path = Application.persistentDataPath + "/Android/Lua/";
    //                if (!Directory.Exists(path))
    //                    Directory.CreateDirectory(path);

    //                FileUtils.CreateFile(path + fileName, bytes);

    //            }
    //#if UNITY_EDITOR
    //            UnityEditor.AssetDatabase.Refresh();
    //#endif
    //            Debug.Log(" 下载完成");
    //            //UnZipTool.UnZipPackage(fileName);
    //        }
    //    }

    //    /// <summary>
    //    /// 从手机的文件管理器中，获取apk文件，并放到persistent目录下
    //    /// </summary>
    //    void GetApkFromFileManager()
    //    {
    //        WebUtils.GetApkFromFile();

    //        //string files = Application.streamingAssetsPath.Replace("jar:", "");
    //        //files = files.Replace("!/assets", "");
    //        //using (WWW www = new WWW(files))
    //        //{
    //        //    while (!www.isDone)
    //        //    {
    //        //        Debug.Log("GetApkFromFileManager:\n=================\n" + www.progress + "\n=================\n");
    //        //    }

    //        //    Debug.Log("\n==================\n==================\n" + Application.streamingAssetsPath + "\n/data/app/" + "\n==================\n==================\n");
    //        //    if (www.isDone)
    //        //    {
    //        //        Debug.Log("\n==================\n==================\n" + Application.streamingAssetsPath + "\n" + files + "\n==================\n==================\n");
    //        //        FileStream output = new FileStream(Application.persistentDataPath + "/base.apk", FileMode.Create, FileAccess.Write);
    //        //        byte[] buffer = www.bytes;
    //        //        //APKMD5 = EncryptProvider.Md5(Encoding.UTF8.GetString(buffer));
    //        //        Debug.Log("\n=======================\n" + APKMD5 + "\n=======================\n");
    //        //        output.Write(buffer, 0, buffer.Length);
    //        //        output.Close();
    //        //        output.Dispose();
    //        //    }
    //        //}
    //    }

    //    void CreateVersionFile()
    //    {
    //        string filename = Path.Combine(Application.persistentDataPath, "config.ini");
    //        if (File.Exists(filename))
    //            return;
    //        string content = "version = " + mReleaseVersion + "." + mMajorVersion;
    //        byte[] buffer = Encoding.UTF8.GetBytes(content);
    //        FileUtils.CreateFile(filename, buffer);
    //    }

}
