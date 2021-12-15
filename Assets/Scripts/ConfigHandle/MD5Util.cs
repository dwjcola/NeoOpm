using System;
using System.IO;
using System.Security.Cryptography;

public class MD5Util 
{
    /// <summary>
    /// �����ַ�����MD5
    /// </summary>
    /// <param name="sDataIn"></param>
    /// <returns></returns>
    public static string GetMD5(string sDataIn)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] bytValue, bytHash;
        bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
        bytHash = md5.ComputeHash(bytValue);
        md5.Clear();
        string sTemp = "";
        for (int i = 0; i < bytHash.Length; i++)
        {
            sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
        }
        return sTemp.ToLower();
    }
    /// <summary>
    /// �����ļ�MD5ֵ
    /// </summary>
    /// <param name="str">��Ҫ������ļ�·��</param>
    /// <returns>MD5ֵ</returns>
    public static string MD5Value(String filepath)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] md5ch;
        using (FileStream fs = File.OpenRead(filepath))
        {
            md5ch = md5.ComputeHash(fs);
        }
        md5.Clear();
        string strMd5 = "";
        for (int i = 0; i < md5ch.Length - 1; i++)
        {
            strMd5 += md5ch[i].ToString("x").PadLeft(2, '0');
        }
        return strMd5;
    }
}
