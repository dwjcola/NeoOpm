using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;
using NeoOPM;


public enum ConfigVersonStatus { 
    BadVersonResule = 0,
   Same = 1,
   Old = 2,
   New = 3,
}
public class ConfigPreload :Singleton<ConfigPreload>
{
 
    string url_root = "http://47.94.233.15/";
    string url_config = "";
    string url_config_list = "";
    string url_config_verson = "";

    string file_suffix_txt = ".txt";
    string file_suffix_json = ".json";


    string datePath = string.Empty;

    StringBuilder stringBuild_fileName;
    StringBuilder stringBuild_path;
    StringBuilder stringBuild_url;

    string verson_file_name = "config_verson.json";
    string verson_list_name = "all_.md5";

    string verson_file_name_tmp = "config_verson_tmp.json";
    string list_file_name_tmp = "all_tmp.md5";

    string p_config_path;
    string p_config_verson_path;
    string p_config_list_path;
    string p_config_dataFile_path;

    string p_config_verson_path_tmp;
    string p_config_list_path_tmp;

    string s_config_path;
    string s_config_verson_path;
    string s_config_list_path;
    string s_config_dataFile_path;

    Action finishDelegate;

    public override void Dispose()
    {
        throw new NotImplementedException();
    }
    public override void Init()
    {
        InitPath();
    }
    public void InitPath()
    {
        url_config = url_root + "config";
        url_config_list = url_root + "config/all.md5";
        url_config_verson = url_root + "config/config_verson.json";

//#if !UNITY_EDITOR
//        datePath = Application.persistentDataPath;
//#endif
//        datePath = Application.streamingAssetsPath;


        stringBuild_fileName = new StringBuilder();
        stringBuild_path = new StringBuilder();
        stringBuild_url = new StringBuilder();



        p_config_path = Application.persistentDataPath + "/config";
        p_config_verson_path = Application.persistentDataPath + "/config/" + verson_file_name;
        p_config_list_path = Application.persistentDataPath + "/config/" + verson_list_name;
        p_config_dataFile_path = Application.persistentDataPath + "/config/configDataFile";


        p_config_verson_path_tmp = Application.persistentDataPath + "/" + verson_file_name_tmp;
        p_config_list_path_tmp = Application.persistentDataPath + "/" + list_file_name_tmp;



        s_config_path = Application.streamingAssetsPath + "/config";
        s_config_verson_path = Application.streamingAssetsPath + "/config/" + verson_file_name;
        s_config_list_path = Application.streamingAssetsPath + "/config/" + verson_list_name;
        s_config_dataFile_path = Application.streamingAssetsPath + "/config/configDataFile";
    }

    public Dictionary<string, string> AnalysisMd5(string path)
    {
        string text = File.ReadAllText(Application.streamingAssetsPath + "/all.md5");
        string[] tmp = text.Split('\n');
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        string[] k_v;
        for (int i = 0; i < tmp.Length; i++)
        {
            k_v = text.Split(',');
            keyValuePairs.Add(k_v[1], k_v[2]);
        }
        return keyValuePairs;
    }
    public void UpdateConfig(Action finishDelegate)
    {

        this.finishDelegate = finishDelegate;
#if !UNITY_EDITOR
        DownMd5AndVerson(()=> {
            CheckPersistentFileStatus();
        });
#else
        if (!NeoOPM.GameEntry.Config.IsUseLocalConfig)
        {
            DownMd5AndVerson(() =>
            {
                CheckPersistentFileStatus();

            });
        }
        else
        {
            if (this.finishDelegate != null)
                this.finishDelegate();
        }
#endif
    }

    public void DownVersonFile(Action action)
    {
        UnityWebManager.LoadFile(new UnityWeb(url_config_verson, p_config_verson_path_tmp, verson_file_name_tmp , action));
    }
    public void DownListFile(Action action)
    {
        UnityWebManager.LoadFile(new UnityWeb(url_config_list, p_config_list_path_tmp, list_file_name_tmp, action));
    }

    public void CheckStreamingAssetsFileStatus()
    {

        // 1. 检查 StreamingAssets 
        if (System.IO.File.Exists(s_config_path) && 
            System.IO.File.Exists(s_config_verson_path) &&
            System.IO.File.Exists(s_config_list_path) && 
            System.IO.File.Exists(s_config_dataFile_path))
        {
            ConfigVersonStatus configVersonStatus = CompareVerson_S_for_CDN();
            switch (configVersonStatus)
            {
                case ConfigVersonStatus.BadVersonResule:
                    // to do 
                    break;
                case ConfigVersonStatus.Same:
                    // copy所有
                    CopyConfigToPByS();
                    break;
                case ConfigVersonStatus.Old:
                    // copy所有 + 更新差异
                    CopyConfigToPByS();
                    BeginDownConfig();
                    break;
                case ConfigVersonStatus.New:
                    // copy所有
                    CopyConfigToPByS();
                    break;
                default:
                    break;
            }
        }
        else
        {
            Logger.Error("StreamingAssets 表数据  不合法！！！");
            BeginDownConfig();
        }
    }
    public void CheckPersistentFileStatus()
    {

        // 1. 检查 Persistent 是否有 config 目录
        if (!System.IO.File.Exists(p_config_path))
        {
#if !UNITY_EDITOR
              //   CheckStreamingAssetsFileStatus();    
#endif
            BeginDownConfig();
        }
        else
        {
            BeginDownConfig();
        }
    }

    
    public ConfigVersonStatus CompareVerson_S_for_CDN()
    {
        Js_ConfigMd5_Verson js_ConfigMd5_Verson_s = ConfigJsonTool.ReadConfigVersonFile(s_config_verson_path);

        Js_ConfigMd5_Verson js_ConfigMd5_Verson_t = ConfigJsonTool.ReadConfigVersonFile(p_config_verson_path_tmp);

        if (js_ConfigMd5_Verson_s != null && js_ConfigMd5_Verson_t != null)
        {
            if (js_ConfigMd5_Verson_s.verson == js_ConfigMd5_Verson_t.verson)
                return ConfigVersonStatus.Same;
            else if (js_ConfigMd5_Verson_s.verson < js_ConfigMd5_Verson_t.verson)
                return ConfigVersonStatus.Old;
            else
            {
                Log.Error("本地Streaming得数值表 版本高于 CDN服务器 ！！！ " + js_ConfigMd5_Verson_s.verson + " >  " + js_ConfigMd5_Verson_t.verson);
                return ConfigVersonStatus.New;
            }
              
        }
        else
        {
            if (js_ConfigMd5_Verson_s == null)
                Log.Error("未找到Streaming远端版本文件 ！！！ " + s_config_verson_path);
            if (js_ConfigMd5_Verson_t == null)
                Log.Error("未找到下载的远端版本文件 ！！！ " + p_config_verson_path_tmp);
            return ConfigVersonStatus.BadVersonResule;
        }
    }
    public void CopyConfigToPByS()
    {
        FileUtils.CopyFolder(s_config_path, p_config_path);

    }

    public void DownMd5AndVerson(Action action)
    {
        //DownVersonFile(()=> { DownListFile(action);});
        DownListFile(action);
    }
    void TryCreatorConfigFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

    }
    public void BeginDownConfig_old()
    {
        TryCreatorConfigFolder(p_config_path);
        Js_ConfigMd5_Verson js_ConfigMd5_Verson_t = ConfigJsonTool.ReadConfigVersonFile(p_config_verson_path_tmp);
        Js_ConfigMd5_List js_ConfigMd5_List_t = ConfigJsonTool.ReadConfigListFile(p_config_list_path_tmp);


        Js_ConfigMd5_Verson js_ConfigMd5_Verson_p = ConfigJsonTool.ReadConfigVersonFile(p_config_verson_path);
        Js_ConfigMd5_List js_ConfigMd5_List_p = ConfigJsonTool.ReadConfigListFile(p_config_list_path);


        Dictionary<string, string> deleteDic = new Dictionary<string, string>();

        Queue<UnityWeb> downWebs = new Queue<UnityWeb>();

        TryCreatorConfigFolder(p_config_dataFile_path);
        if (js_ConfigMd5_Verson_p != null && js_ConfigMd5_List_p != null)
        {
            if (js_ConfigMd5_Verson_t.verson < js_ConfigMd5_Verson_p.verson)
            {
                Log.Error("？？？ 异常 本地数值表verson 版本 高于 cdn");
            }
            else
            {
                foreach (KeyValuePair<string, string> kv in js_ConfigMd5_List_t.configMd5)
                {
                    string tmpMDF;
                    if (js_ConfigMd5_List_p.configMd5.TryGetValue(kv.Key, out tmpMDF))
                    {

                        GetConfigFileMD5(kv.Key,out tmpMDF);
                       
                        if (tmpMDF != kv.Value)
                        {
                            downWebs.Enqueue(CreateDownWeb(kv.Key));
                        }
                        js_ConfigMd5_List_p.configMd5.Remove(kv.Key);
                    }
                    else
                    {
                        downWebs.Enqueue(CreateDownWeb(kv.Key));
                    }

                }
                foreach (KeyValuePair<string, string> kv in js_ConfigMd5_List_p.configMd5)
                {
                    string tmpMDF;
                    if (js_ConfigMd5_List_p.configMd5.TryGetValue(kv.Key, out tmpMDF))
                    {
                        // nothing to do;
                    }
                    else
                    {
                        stringBuild_path.Clear();
                       deleteDic.Add(kv.Key, stringBuild_path.AppendFormat("{0}/{1}{2}", p_config_dataFile_path, kv.Key, file_suffix_txt).ToString());
                    }
                }
            }
        }
        else
        {
            // 没有 删除 config下所有文件 
            // 然后更新 所有
            FileUtils.DeleteAllFile(p_config_path);
            foreach (KeyValuePair<string, string> kv in js_ConfigMd5_List_t.configMd5)
            {
                downWebs.Enqueue(CreateDownWeb(kv.Key));
            }
        }
        DeleteConfig(deleteDic);
        DownConfig(downWebs);
    }
    public void BeginDownConfig()
    {
        TryCreatorConfigFolder(p_config_path);


        Dictionary<string, string> Md5_Dic_t = AnalysisMd5(p_config_list_path_tmp);

        Dictionary<string, string> deleteDic = new Dictionary<string, string>();

        Queue<UnityWeb> downWebs = new Queue<UnityWeb>();


        TryCreatorConfigFolder(p_config_dataFile_path);

        if (!File.Exists(p_config_list_path))
        {
            FileUtils.DeleteAllFile(p_config_path);
            foreach (KeyValuePair<string, string> kv in Md5_Dic_t)
            {
                downWebs.Enqueue(CreateDownWeb(kv.Key));
            }
        }
        else
        {
            Dictionary<string, string> Md5_Dic_p = AnalysisMd5(p_config_list_path_tmp);
            foreach (KeyValuePair<string, string> kv in Md5_Dic_t)
            {
                string tmpMDF;
                if (Md5_Dic_p.TryGetValue(kv.Key, out tmpMDF))
                {

                    GetConfigFileMD5(kv.Key, out tmpMDF);

                    if (tmpMDF != kv.Value)
                    {
                        downWebs.Enqueue(CreateDownWeb(kv.Key));
                    }
                    Md5_Dic_p.Remove(kv.Key);
                }
                else
                {
                    downWebs.Enqueue(CreateDownWeb(kv.Key));
                }

            }
            foreach (KeyValuePair<string, string> kv in Md5_Dic_p)
            {
                string tmpMDF;
                if (Md5_Dic_p.TryGetValue(kv.Key, out tmpMDF))
                {
                    // nothing to do;
                }
                else
                {
                    stringBuild_path.Clear();
                    deleteDic.Add(kv.Key, stringBuild_path.AppendFormat("{0}/{1}{2}", p_config_dataFile_path, kv.Key, file_suffix_txt).ToString());
                }
            }
        }
     
        DeleteConfig(deleteDic);
        DownConfig(downWebs);
    }
    UnityWeb CreateDownWeb(string name)
    {
        stringBuild_fileName.Clear();
        stringBuild_fileName.AppendFormat("{0}{1}", name, file_suffix_txt);
        stringBuild_url.Clear();
        stringBuild_url.AppendFormat("{0}/{1}{2}", url_config, name, file_suffix_txt);
        stringBuild_path.Clear();
        stringBuild_path.AppendFormat("{0}/{1}{2}", p_config_dataFile_path, name, file_suffix_txt);
        return new UnityWeb(stringBuild_url.ToString(), stringBuild_path.ToString(), stringBuild_fileName.ToString());
    }

    void DeleteConfig(Dictionary<string, string> deleteDic)
    {
        foreach (KeyValuePair<string, string> kv in deleteDic)
        {
            File.Delete(kv.Value);
        }

    }
    void DownConfig(Queue<UnityWeb> downWebs)
    {
        UnityWebManager.LoadFile(downWebs, ConfigUpdateFinish);
    }
    
   
   void ConfigUpdateFinish()
    {
        // to do
        //替换两个文件得内容
        File.Delete(p_config_verson_path);
        File.Delete(p_config_list_path);
       
        /*FileUtil.CopyFileOrDirectory(p_config_verson_path_tmp, p_config_verson_path);
        FileUtil.CopyFileOrDirectory(p_config_list_path_tmp, p_config_list_path);
        if (finishDelegate != null)
            finishDelegate();*/
    }

    void GetConfigFileMD5(string fileName,out string tmpMDF)
    {
        stringBuild_path.Clear();
        stringBuild_path.AppendFormat("{0}/{1}{2}", p_config_dataFile_path, fileName, file_suffix_txt);
        tmpMDF =  MD5Util.MD5Value(stringBuild_path.ToString());
    }
}


