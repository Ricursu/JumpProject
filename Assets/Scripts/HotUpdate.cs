using NETCore.Encrypt;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HotUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public static int mReleaseVersion = 1;
    public static int mMajorVersion = 1;
    public static int mMinorVersion = 3;

    public static string APKMD5 = "";

    /// <summary>
    /// 检查更新
    /// </summary>
    void Awake()
    {
        //string packageName = "com." + Application.companyName + "." + Application.productName;

        GetApkFromFileManager();
        WebUtils.GetFileFromServer("version.zip");
        //UnZipTool.UnZipApk(Application.persistentDataPath + "/base.apk");
        //UnZipTool.UnZip("version.zip");
        DiffUtils.ReductionFile(Path.Combine(Application.persistentDataPath, "assets/Lua"), Path.Combine(Application.persistentDataPath, "version/filelist.txt"));
    }

    /// <summary>
    /// 通过UnityWebRequest从服务器端下载文件名为filename的文件
    /// </summary>
    /// <param name="fileName">需要下载的文件名</param>
    /// <returns></returns>
    void ReadAssetBundle(string fileName)
    {
        string url = "http://10.230.17.74/" + fileName;
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();
        while (!request.isDone)
        {
            Debug.Log("ReadAssetBundle:\n====================\n" + request.downloadProgress + "\n====================");

        }

        if (request.isDone)                  //下载完成
        {
            byte[] bytes = request.downloadHandler.data;

            if (Application.platform == RuntimePlatform.Android)
            {
                string path = Application.persistentDataPath + "/Android/Lua/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                FileUtils.CreateFile(path + fileName, bytes);

            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            Debug.Log(" 下载完成");
            UnZipTool.UnZipPackage(fileName);
        }
    }

    /// <summary>
    /// 从手机的文件管理器中，获取apk文件，并放到persistent目录下
    /// </summary>
    void GetApkFromFileManager()
    {
        string files = Application.streamingAssetsPath.Replace("jar:", "");
        files = files.Replace("!/assets", "");
        using (WWW www = new WWW(files))
        {
            while (!www.isDone)
            {
                Debug.Log("GetApkFromFileManager:\n=================\n" + www.progress + "\n=================\n");
            }

            Debug.Log("\n==================\n==================\n" + Application.streamingAssetsPath + "\n/data/app/" + "\n==================\n==================\n");
            if (www.isDone)
            {
                Debug.Log("\n==================\n==================\n" + Application.streamingAssetsPath + "\n" + files + "\n==================\n==================\n");
                FileStream output = new FileStream(Application.persistentDataPath + "/base.apk", FileMode.Create, FileAccess.Write);
                byte[] buffer = www.bytes;
                //APKMD5 = EncryptProvider.Md5(Encoding.UTF8.GetString(buffer));
                Debug.Log("\n=======================\n" + APKMD5 + "\n=======================\n");
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                output.Dispose();
            }
        }
    }


}
