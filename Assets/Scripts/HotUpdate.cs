using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class HotUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public static int mReleaseVersion = 1;
    public static int mMajorVersion = 1;
    public static int mMinorVersion = 3;

    /// <summary>
    /// 检查更新
    /// </summary>
    void Awake()
    {
        string packageName = "com." + Application.companyName + "." + Application.productName;
        Debug.Log("\n==================\n==================\n" + Application.streamingAssetsPath + "\n/data/app/" + packageName + "\n==================\n==================\n");


        StartCoroutine(GetApkFromFileManager());

        //foreach(string filename in files)
        //{
        //    if(filename.Contains(packageName))
        //        if (File.Exists(filename + "/base.apk"))
        //            Debug.Log("\n==================\n==================\n Exixt Base.Apk \n==================\n==================\n");
        //1}


        ///全量更新
        //UnZipTool.CreateVersionFile()
        //StartCoroutine(ReadAssetBundle("VERSION"));
        //string[] downloadList = FileUtils.ReadFileLines(Application.persistentDataPath + "/Android/Lua/VERSION");
        //foreach(string str in downloadList)
        //{
        //    string temp = str;
        //    Debug.Log("                                        " + temp + "                                        ");
        //    temp = temp.Replace('[', ' ');
        //    temp = temp.Replace(']', ' ');
        //    temp = temp.Trim();
        //    Debug.Log("                                        " + temp + "                                        ");
        //    StartCoroutine(ReadAssetBundle(temp + ".zip"));
        //}
        //StartCoroutine(ReadAssetBundle("luascript.unity3d.manifest"));
    }


    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 通过UnityWebRequest从服务器端下载文件名为filename的文件
    /// </summary>
    /// <param name="fileName">需要下载的文件名</param>
    /// <returns></returns>
    IEnumerator ReadAssetBundle(string fileName)
    {
        string url = "http://10.230.17.74/" + fileName;
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();
        while (!request.isDone)
        {
            //Debug.Log("====================\n" + request.downloadProgress + "\n====================");
            yield return 1;

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

    IEnumerator GetApkFromFileManager()
    {
        string files = Application.streamingAssetsPath.Replace("jar:", "");
        files = files.Replace("!/assets", "");
        using (WWW www = new WWW(files))
        {
            yield return www;
            Debug.Log("\n==================\n==================\n" + Application.streamingAssetsPath + "\n/data/app/" + "\n==================\n==================\n");
            if (www.isDone)
            {
                Debug.Log("\n==================\n==================\n" + Application.streamingAssetsPath + "\n" + files + "\n==================\n==================\n");
                FileStream output = new FileStream(Application.persistentDataPath + "/base.apk", FileMode.Create, FileAccess.Write);
                byte[] buffer = www.bytes;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                output.Dispose();
            }
        }
    }
}
