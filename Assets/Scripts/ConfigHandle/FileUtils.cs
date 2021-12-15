using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUtils 
{
    public static void GetDirs(string path, ref Dictionary<string, string> dirs)
    {
        foreach (string dirpath in Directory.GetFiles(path))
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(dirpath);
            if (dirs.ContainsKey(fileName))
            {
                Debug.LogError("重复表名 ！！！" + fileName);
            }
            else
            {
                dirs.Add(fileName, dirpath);
                Debug.Log(fileName + "   :   " + dirpath);
            }
        }

    }

    // Start is called before the first frame update
    private static void GetDirs(string path, ref List<string> dirs)
    {
        foreach (string dirpath in Directory.GetFiles(path))
        {
            //获取文件名中包含“_vcc”的文件路径
            if (System.IO.Path.GetFileNameWithoutExtension(dirpath).Contains("_vcc"))
            {
                //查询获得的文件路径字符串以“Assets”开头
                dirs.Add(dirpath.Substring(dirpath.IndexOf("Assets")));
                Debug.Log(dirpath.Substring(dirpath.IndexOf("Assets")));
            }
        }
        //递归调用自身来遍历所有子文件夹
        if (Directory.GetDirectories(path).Length > 0)
        {
            foreach (string subDir in Directory.GetDirectories(path))
            {
                GetDirs(subDir, ref dirs);
            }
        }
    }
    /// <summary>
    /// 复制文件夹所有文件
    /// </summary>
    /// <param name="sourcePath">源目录</param>
    /// <param name="destPath">目的目录</param>
    public static void CopyFolder(string sourcePath, string destPath)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(destPath))
            {
                //目标目录不存在则创建
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("创建目标目录失败：" + ex.Message);
                }
            }
            //获得源文件下所有文件
            List<string> files = new List<string>(Directory.GetFiles(sourcePath));
            files.ForEach(c =>
            {
                string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                File.Copy(c, destFile, true);//覆盖模式
            });
            //获得源文件下所有目录文件
            List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
            folders.ForEach(c =>
            {
                string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                //采用递归的方法实现
                CopyFolder(c, destDir);
            });

        }
    }

    /// <summary>
    /// 删除指定文件目录下的所有文件
    /// </summary>
    /// <param name="fullPath">文件路径</param>
    public static bool DeleteAllFile(string fullPath)
    {
        //获取指定路径下面的所有资源文件  然后进行删除
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);      
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                string FilePath = fullPath + "/" + files[i].Name;
                File.Delete(FilePath);
            }
            return true;
        }
        return false;
    }
}


