using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class WebUtils
{
    public static string IP = "http://10.13.9.210/";

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
        string path = WebUtils.IP + filename;    // "http://" + WebUtils.GetIPAddress() + "/" +
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


    /// <summary>
    /// 判断服务中是否有需要的文件
    /// </summary>
    /// <param name="url">需要文件的url地址</param>
    /// <returns></returns>
    public static bool IsExistFileInServer(string url)
    {
        try
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "HEAD";
            request.Timeout = 1000;
            return ((HttpWebResponse)request.GetResponse()).StatusCode == HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 获取服务器端的文件信息
    /// </summary>
    /// <param name="filename">读取的文件名</param>
    /// <returns></returns>
    public static byte[] GetByteFromServer(string filename)
    {
        //Debug.Log(WebUtils.GetIPAddress());
        string path = WebUtils.IP + filename;    // "http://" + WebUtils.GetIPAddress() + "/" +
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
            return null;
        }
        if (request.isDone)                  //下载完成
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            return request.downloadHandler.data;
            Debug.Log(" 下载完成");
            //UnZipTool.UnZipPackage(fileName);
        }
        return null;
    }
}
