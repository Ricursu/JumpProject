using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileUtils
{
    /// <summary>
    /// 将传入的writeString写入带绝对路径的文件名为filename的文件中
    /// </summary>
    /// <param name="filename">带绝对路径的文件</param>
    /// <param name="checkString">需要写入的文件的字符串</param>
    /// <returns></returns>
    public static bool WriteFileAppend(string filename, string writeString)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(writeString + "\n");
        FileStream fs = null;
        try
        {
            if (File.Exists(filename))
            {
                fs = File.Open(filename, FileMode.Append);
            }
            else
            {
                fs = File.Open(filename, FileMode.Create);
            }
            fs.Write(buffer, 0, buffer.Length);
        }catch(Exception e)
        {
            Debug.Log(e.GetType() + "\n" + e.GetBaseException());
            return false;
        }
        finally
        {
            fs.Close();
            fs.Dispose();
        }
        return true;
    }

    public static bool CreateFile(string filename, byte[] buffer)
    {
        FileStream fs = null;
        try
        {
            FileInfo fileInfo = new FileInfo(filename);
            fs = fileInfo.Create();
            fs.Write(buffer, 0, buffer.Length);
        }catch(Exception e)
        {

            return false;
        }
        finally
        {
            fs.Flush();
            fs.Close();
            fs.Dispose();
        }
        return true;
    }

    public static string[] ReadFileLines(string filename)
    {
        string[] content = null;
        FileStream fs = null;
        try
        {
            content = File.ReadAllLines(filename);
        }catch(Exception e)
        {
            Debug.Log(e.GetType() + " " + e.GetBaseException());
        }
        return content;
    }

}
