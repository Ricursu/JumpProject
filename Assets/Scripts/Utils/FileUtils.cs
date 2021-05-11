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

    /// <summary>
    /// 创建新的文件，并写入数据
    /// </summary>
    /// <param name="filename">创建的文件名</param>
    /// <param name="buffer">写入的数据的字节流</param>
    /// <returns></returns>
    public static bool CreateFile(string filename, byte[] buffer)
    {
        FileStream fs = null;
        try
        {
            if (File.Exists(filename))
                File.Delete(filename);
            fs = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);
            fs.Write(buffer, 0, buffer.Length);
        }catch(Exception e)
        {

            return false;
        }
        finally
        {
            if (fs != null)
            {
                fs.Flush();
                fs.Close();
                fs.Dispose();
            }
        }
        return true;
    }

    /// <summary>
    /// 按行读取文件的所有信息
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
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


    public static byte[] ReadFileBytes(string filename)
    {
        byte[] buffer = null;
        FileStream input = null;
        try
        {
            input = new FileStream(filename, FileMode.Open, FileAccess.Read);
            buffer = new byte[input.Length];
            input.Read(buffer, 0, buffer.Length);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (input != null)
            {
                input.Dispose();
                input.Close();
            }
        }

        return buffer;
    }

    public static bool FileExists(string filename)
    {
        return File.Exists(filename);
    }

    public static bool DirectoryExists(string filename)
    {
        return Directory.Exists(filename);
    }

    public static void DirectoryDelete(string filename, bool recursive)
    {
        Directory.Delete(filename, recursive);
    }

    public static string[] DirectoryGetFiles(string filename)
    {
        return Directory.GetFiles(filename);
    }


    public static void CopyFileToPath(string oldFilePath, string newFilePath)
    {
        byte[] buffer = ReadFileBytes(oldFilePath);
        CreateFile(newFilePath, buffer);
    }

}
