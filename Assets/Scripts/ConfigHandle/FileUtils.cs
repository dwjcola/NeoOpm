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
                Debug.LogError("�ظ����� ������" + fileName);
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
            //��ȡ�ļ����а�����_vcc�����ļ�·��
            if (System.IO.Path.GetFileNameWithoutExtension(dirpath).Contains("_vcc"))
            {
                //��ѯ��õ��ļ�·���ַ����ԡ�Assets����ͷ
                dirs.Add(dirpath.Substring(dirpath.IndexOf("Assets")));
                Debug.Log(dirpath.Substring(dirpath.IndexOf("Assets")));
            }
        }
        //�ݹ���������������������ļ���
        if (Directory.GetDirectories(path).Length > 0)
        {
            foreach (string subDir in Directory.GetDirectories(path))
            {
                GetDirs(subDir, ref dirs);
            }
        }
    }
    /// <summary>
    /// �����ļ��������ļ�
    /// </summary>
    /// <param name="sourcePath">ԴĿ¼</param>
    /// <param name="destPath">Ŀ��Ŀ¼</param>
    public static void CopyFolder(string sourcePath, string destPath)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(destPath))
            {
                //Ŀ��Ŀ¼�������򴴽�
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("����Ŀ��Ŀ¼ʧ�ܣ�" + ex.Message);
                }
            }
            //���Դ�ļ��������ļ�
            List<string> files = new List<string>(Directory.GetFiles(sourcePath));
            files.ForEach(c =>
            {
                string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                File.Copy(c, destFile, true);//����ģʽ
            });
            //���Դ�ļ�������Ŀ¼�ļ�
            List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
            folders.ForEach(c =>
            {
                string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                //���õݹ�ķ���ʵ��
                CopyFolder(c, destDir);
            });

        }
    }

    /// <summary>
    /// ɾ��ָ���ļ�Ŀ¼�µ������ļ�
    /// </summary>
    /// <param name="fullPath">�ļ�·��</param>
    public static bool DeleteAllFile(string fullPath)
    {
        //��ȡָ��·�������������Դ�ļ�  Ȼ�����ɾ��
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


