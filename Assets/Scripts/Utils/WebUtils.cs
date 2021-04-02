using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class WebUtils
{
    public static string GetIPAddress()
    {
        string ipaddr = null;
        var strHostName = System.Net.Dns.GetHostName();
        var ipEntry = System.Net.Dns.GetHostEntry(strHostName);
        var addr = ipEntry.AddressList;
        ipaddr = addr[1].ToString();
        return ipaddr;
    }

    /// <summary>
    /// 获取服务器端文件
    /// </summary>
    /// <param name="filename">获取文件名</param>
    public static void GetFileFromServer(string filename)
    {
        //Debug.Log(WebUtils.GetIPAddress());
        string path = "http://10.230.17.74/" + filename;    // "http://" + WebUtils.GetIPAddress() + "/" +
        Debug.Log(path);
        UnityWebRequest request = UnityWebRequest.Get(path);
        request.SendWebRequest();
        while (!request.isDone)
        {
            Debug.Log("GetFileFromServer:\n====================\n" + request.downloadProgress + "\n====================");
        }
        if (request.isNetworkError)
        {
            Debug.Log("\n=======\n\n===下载失败====\n\n=======\n");
            return;
        }
        if (request.isDone)                  //下载完成
        {
            byte[] bytes = request.downloadHandler.data;

            if (Application.platform == RuntimePlatform.Android)
            {
                string savePath = Application.persistentDataPath;
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                FileUtils.CreateFile(Path.Combine(savePath, filename), bytes);

            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            Debug.Log(" 下载完成");
            //UnZipTool.UnZipPackage(fileName);
        }
    }
}
