using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityGameFramework.Runtime;

public class Js_ConfigMd5_List
{
    public Dictionary<string, string> configMd5 = new Dictionary<string, string>();

    public Js_ConfigMd5_List()
    {
    }

    public Js_ConfigMd5_List(Dictionary<string, string> configMd5)
    {
        this.configMd5 = configMd5;
    }
}

public class Js_ConfigMd5_Verson
{
    public string mateMD5 = string.Empty;
    public int verson = default;
    public Js_ConfigMd5_Verson()
    {
    }

    public Js_ConfigMd5_Verson(string mateMD5, int verson)
    {
        this.mateMD5 = mateMD5;
        this.verson = verson;
    }
}
public class ConfigJsonTool 
{
    public static void TmpWriterConfigMDFFile(string path, Dictionary<string, string> configMd5)
    {
        Js_ConfigMd5_List js_Config_Md5 = new Js_ConfigMd5_List(configMd5);
        string json = JsonMapper.ToJson(js_Config_Md5);      //利用JsonMapper将乐队信息转为Json格式的文本
        StreamWriter sw = new StreamWriter(path);	//利用写入流创建文件
        sw.Write(json);		//写入数据
        sw.Close();		//关闭流
        sw.Dispose();	//销毁流
    }
    public static void CreateConfigMDFFile_MDFFile(string configMDFPath, string configMDF_MDF_Path)
    {
        Js_ConfigMd5_Verson js_ConfigMDF_Md5 = new Js_ConfigMd5_Verson(MD5Util.MD5Value(configMDFPath),0);
        string json = JsonMapper.ToJson(js_ConfigMDF_Md5);      //利用JsonMapper将乐队信息转为Json格式的文本
        StreamWriter sw = new StreamWriter(configMDF_MDF_Path);	//利用写入流创建文件
        sw.Write(json);		//写入数据
        sw.Close();		//关闭流
        sw.Dispose();	//销毁流
    }


    public static Js_ConfigMd5_Verson ReadConfigVersonFile(string config_verson_path)
    {
        if (!File.Exists(config_verson_path))
        {
            Log.Info("文件不存在！！！   " + config_verson_path);
            return null;
        }
        string text = File.ReadAllText(config_verson_path);
        return JsonMapper.ToObject<Js_ConfigMd5_Verson>(text);
    }
    public static Js_ConfigMd5_List ReadConfigListFile(string config_list_path)
    {
        if (!File.Exists(config_list_path))
        {
            Log.Info("文件不存在！！！   " + config_list_path);
            return null;
        }
        string text = File.ReadAllText(config_list_path);
        return JsonMapper.ToObject<Js_ConfigMd5_List>(text);
    }
}
