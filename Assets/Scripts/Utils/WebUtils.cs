using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class WebUtils
{
    public static string IP = "http://192.168.3.30/";
    public static bool isDone = false;
    public static string processText = "0";
    public static string processSlider = "0";

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
        string savePath = Application.persistentDataPath;
        Thread thread = new Thread(delegate () { ThreadDownLoad(filename, savePath); });
        //开启子线程
        thread.IsBackground = true;
        thread.Start();
        //        //Debug.Log(WebUtils.GetIPAddress());
        //        string path = WebUtils.IP + filename;    // "http://" + WebUtils.GetIPAddress() + "/" +
        //        Debug.Log(path);
        //        WWW request = new WWW(path);
        //        while (!request.isDone)
        //        {
        //            HotUpdate.ChangeSlider(System.Convert.ToString(request.progress));
        //            HotUpdate.ChangeLoadingprogress(System.Convert.ToString(request.progress * 100));
        //            //Debug.LogWarning("GetFileFromServer:\n====================\n" + request.progress + "\n====================");
        //        }
        //        if (request.error != null)
        //        {
        //            Debug.Log("\n=======\n\n===下载失败====\n\n=======\n");
        //            return;
        //        }
        //        if (request.isDone)                  //下载完成
        //        {
        //            byte[] bytes = request.bytes;

        //            //if (Application.platform == RuntimePlatform.Android)
        //            //{
        //                string savePath = Application.persistentDataPath;
        //                if (!Directory.Exists(savePath))
        //                    Directory.CreateDirectory(savePath);
        //                FileUtils.CreateFile(Path.Combine(savePath, filename), bytes);

        //            //}
        //#if UNITY_EDITOR
        //            UnityEditor.AssetDatabase.Refresh();
        //#endif
        //            Debug.Log(" 下载完成");
        //            //UnZipTool.UnZipPackage(fileName);
        //        }
    }

    public static void ThreadDownLoad(string filename, string savePath)
    {
        isDone = false;
        string path = WebUtils.IP + filename;
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }


        //这是要下载的文件名，比如从服务器下载a.zip到D盘，保存的文件名是test
        //string filePath = savePath + "/test";

        Debug.Log("12345  " + path);
        //使用流操作文件
        FileStream fs = new FileStream(Path.Combine(savePath, filename), FileMode.OpenOrCreate, FileAccess.Write);
        //获取文件现在的长度
        long fileLength = fs.Length;

        HttpWebRequest request = HttpWebRequest.Create(path) as HttpWebRequest;
        request.AddRange(fileLength);
        Stream stream = request.GetResponse().GetResponseStream();

        //获取下载文件的总长度
        Debug.Log(fileLength);  
        long totalLength = GetLength(path);
        Debug.Log(totalLength);


        //如果没下载完
        if (fileLength < totalLength)
        {

            //断点续传核心，设置本地文件流的起始位置
            fs.Seek(fileLength, SeekOrigin.Current);


            //request.Method = "POST";

            Debug.Log(fileLength);
            //断点续传核心，设置远程访问文件流的起始位置


            byte[] buffer = new byte[8];
            //使用流读取内容到buffer中
            //注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
            int length = stream.Read(buffer, 0, buffer.Length);
            while (length > 0)
            {
                //将内容再写入本地文件中
                fs.Write(buffer, 0, length);
                //计算进度
                fileLength += length;
                float process = (float)fileLength / (float)totalLength;
                processSlider = process.ToString();
                processText = (process * 100).ToString("0.00");
                //类似尾递归
                length = stream.Read(buffer, 0, buffer.Length);
                Thread.Sleep(50);
                //yield return false;
            }
            stream.Dispose();
            stream.Close();
        }
        else
        {
            HotUpdate.ChangeSlider("1");
        }
        fs.Close();
        fs.Dispose();
        isDone = true;
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
        WWW request = new WWW(path);
        while (!request.isDone)
        {
            HotUpdate.ChangeSlider(System.Convert.ToString(request.progress));
            HotUpdate.ChangeLoadingprogress(System.Convert.ToString(request.progress * 100));
            //Debug.LogWarning("GetFileFromServer:\n====================\n" + request.progress + "\n====================");
        }
        if (request.error != null)
        {
            Debug.Log("\n=======\n\n===下载失败====\n\n=======\n");
            return null;
        }
        if (request.isDone)                  //下载完成
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            return request.bytes;
            Debug.Log(" 下载完成");
            //UnZipTool.UnZipPackage(fileName);
        }
        return null;
    }

    public static void GetApkFromFile()
    {
        string path1 = Application.streamingAssetsPath;
        string path2 = Application.persistentDataPath;

        string files = path1.Replace("jar:", "");
        files = files.Replace("!/assets", "");
        WWW www = new WWW(files);
        ThreadGetApk(path1, path2, www);
        //Thread thread = new Thread(delegate () { ThreadGetApk(path1, path2); });
        ////开启子线程
        //thread.IsBackground = true;
        //thread.Start();
    }


    /// <summary>
    /// 获取下载文件的大小
    /// </summary>
    /// <returns>The length.</returns>
    /// <param name="url">URL.</param>
    public static long GetLength(string url)
    {
        UnityEngine.Debug.Log(url);

        HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
        request.Method = "HEAD";
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        long size = response.ContentLength;

        response.Close();
        request.Abort();
        return size;
    }

    public static void ThreadGetApk(string streamingAssetsPath, string persistentDataPath, WWW www)
    {
        isDone = false;

        //string path = streamingAssetsPath.Replace("jar:", "");
        //path = path.Replace("!/assets", "");


        ////使用流操作文件
        //FileStream fs = new FileStream(Path.Combine(persistentDataPath, "base.apk"), FileMode.OpenOrCreate, FileAccess.Write);
        ////获取文件现在的长度
        //long fileLength = fs.Length;
        //HttpWebRequest request = HttpWebRequest.Create(path) as HttpWebRequest;
        //request.AddRange(fileLength);
        //Stream stream = request.GetResponse().GetResponseStream();

        ////获取下载文件的总长度
        //Debug.Log(fileLength);
        //long totalLength = GetLength(path);
        //Debug.Log(totalLength);


        ////如果没下载完
        //if (fileLength < totalLength)
        //{

        //    //断点续传核心，设置本地文件流的起始位置
        //    fs.Seek(fileLength, SeekOrigin.Current);


        //    //request.Method = "POST";

        //    Debug.Log(fileLength);
        //    //断点续传核心，设置远程访问文件流的起始位置


        //    byte[] buffer = new byte[8];
        //    //使用流读取内容到buffer中
        //    //注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
        //    int length = stream.Read(buffer, 0, buffer.Length);
        //    while (length > 0)
        //    {
        //        //将内容再写入本地文件中
        //        fs.Write(buffer, 0, length);
        //        //计算进度
        //        fileLength += length;
        //        float process = (float)fileLength / (float)totalLength;
        //        processSlider = process.ToString();
        //        processText = (process * 100).ToString("0.00");
        //        //类似尾递归
        //        length = stream.Read(buffer, 0, buffer.Length);
        //        Thread.Sleep(50);
        //        //yield return false;
        //    }
        //    stream.Dispose();
        //    stream.Close();
        //}
        //else
        //{
        //    processSlider = "1";
        //}
        //fs.Close();
        //fs.Dispose();

        //string files = streamingAssetsPath.Replace("jar:", "");
        //files = files.Replace("!/assets", "");
        //WWW www = new WWW(files);


        while (!www.isDone)
        {
            //Debug.LogWarning("GetFileFromServer:\n====================\n" + www.progress + "\n====================");
            float process = www.progress;
            processSlider = process.ToString();
            processText = (process * 100).ToString("0.00");
            Thread.Sleep(100);
        }

        if (www.isDone)
        {
            FileStream output = new FileStream(persistentDataPath + "/base.apk", FileMode.Create, FileAccess.Write);
            byte[] buffer = www.bytes;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            output.Dispose();
        }

        isDone = true;
    }

}
