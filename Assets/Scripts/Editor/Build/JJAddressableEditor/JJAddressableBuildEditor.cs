using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Build;
using System.IO;
using System.Text;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using LitJson;
using NeoOPM;

public class JJAddressableBuildEditor : Editor
{
    public const string JJAddressableTools = @"SLGTools/JJAddressables/";
    const string RemoteRootPath = "ServerData/[BuildTarget]";
    [MenuItem("SLGTools / JJAddressables / ReBuildAASBundle")]
    public static void ReBuildBundle()
    {
        var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);

        settings.BuildRemoteCatalog = false;//暂时修改为不连接更新
        settings.DisableCatalogUpdateOnStartup = true;
        ProjectConfigData.PostProfilerEvents = false;
        

        GenerateProfiles();
        ClearRemoteBuildPath();//重新打bundle清除旧有的服务器目录下的bundle包
        AutoSetGroup();///重新打bundle需要把分组重新设置,把打更新拆分的组再改回来
        AddressableAssetSettings.BuildPlayerContent();
        Debug.Log("BuildAddrressable 完成");
    }
    [MenuItem("SLGTools / JJAddressables / BuildUpdateBundle")]
    public static void BuildUpdateBundle()
    {
        GenerateProfiles();
        BuildContentUpdate();
        Debug.Log("BuildContentUpdate 完成");
    }


    #region 打最新的资源包
    const string aasProfilePath = "Assets/Resource_MS/SoTables/AdressablesConfigProfile.asset";
    /// <summary>
    /// 生成本地和远端打包路径
    /// 在streamingAssets目录写入一个包含更新地址的json文件
    /// </summary>
    [MenuItem("SLGTools/JJAddressables/GenerateProfiles")]
    public static void GenerateProfiles()
    {
        var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
        if (!settings.BuildRemoteCatalog)//不打remote包时不需要处理了
            return;
        string path = Path.Combine(Application.streamingAssetsPath, PlatformConfig.RemoteConfigPath);
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        JsonData data = new JsonData();


#if UNITY_ANDROID || UNITY_IOS
        string profileName = JenkinsAdapter.IsDebug ? "Develop" :"Product";
#else
        string profileName = "Develop";
#endif
        //待处理 重新读表处理 dwj 2021-12-2
        /*var profileTable =AssetDatabase.LoadAssetAtPath<AdressablesConfigProfileTable>(aasProfilePath);
        foreach (var item in profileTable.TableDic)
        {
            var pName = item.Value.ProfileName;
            if (pName != profileName)
                continue;
            var remoteLoadPath = item.Value.RemoteLoadPath;
            if (settings.profileSettings.GetProfileId(pName) == "")
            {
                settings.profileSettings.AddProfile(pName, pName);
            }

            data[PlatformConfig.kRemoteLoadPath] = settings.profileSettings.EvaluateString(settings.profileSettings.GetProfileId(pName), remoteLoadPath);

            settings.profileSettings.SetValue(settings.profileSettings.GetProfileId(pName), AddressableAssetSettings.kRemoteLoadPath, $"{NeoOPM.PlatformConfig.RemoteBundleURL}");
            settings.profileSettings.SetValue(settings.profileSettings.GetProfileId(pName), AddressableAssetSettings.kRemoteBuildPath, $"{RemoteRootPath}/Update-{System.DateTime.Now:yy.MM.dd.HH.mm.ss}");
        }

        File.WriteAllText(path, JsonMapper.ToJson(data));

        settings.activeProfileId = settings.profileSettings.GetProfileId(profileName);
        Debug.Log("配置 更新地址 完成");
        EditorUtility.SetDirty(profileTable);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();*/
    }
    /// <summary>
    /// 生成打包的分组信息
    /// </summary>
    [MenuItem("SLGTools/JJAddressables/AutoSetGroup")]
    public static void AutoSetGroup()
    {
        var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
        settings.RemoteCatalogBuildPath.SetVariableByName(settings, AddressableAssetSettings.kRemoteBuildPath);
        settings.RemoteCatalogLoadPath.SetVariableByName(settings, AddressableAssetSettings.kRemoteLoadPath);

        JJAASBuildScript.GroupsFromJson();

        Debug.Log("设置 Group Label 完成");
    }
    public static void ClearGroupEntries(AddressableAssetGroup group)
    {
        List<string> guids = new List<string>();
        foreach (var item in group.entries)
        {
            if (item != null)
            {
                guids.Add(item.guid);
            }
        }
        var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
        for (int i = 0; i < guids.Count; i++)
        {
            settings.RemoveAssetEntry(guids[i]);
        }

    }
    public static void DeleteNoEntriesGroup()
    {
        var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
        var oldGroups = settings.groups;
        var toDeletedGroups = new List<AddressableAssetGroup>();
        //for (var i = 0; i < oldGroups.Count; i++)
        //{
        //    var oldGroupName = oldGroups[i].Name;
        //    if (oldGroupName == "Built In Data" || oldGroups[i].IsDefaultGroup()) continue;
        //    toDeletedGroups.Add(oldGroups[i]);
        //}
        for (var i = 0; i < oldGroups.Count; i++)
        {
            var oldGroupName = oldGroups[i].Name;
            if (oldGroupName == "Built In Data" || oldGroups[i].IsDefaultGroup()) continue;
            if (oldGroups[i].entries.Count <= 0)
                toDeletedGroups.Add(oldGroups[i]);
        }
        if (toDeletedGroups.Count > 0)
        {
            foreach (var group in toDeletedGroups)
            {
                settings.RemoveGroup(group);
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("删除废弃Group完成");
        
    }
    #endregion

    #region 打更新资源包
    public static void BuildContentUpdate()
    {
        JJAASBuildScript.FixGroupAfterMerge();
        JJAASBuildScript.CheckForUpdateContent();
        JJAASBuildScript.UpdateAPreviousBuild();
    }
    
    #endregion

    public static void ChangeActivePlayMode()
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
        int index = -1;
        for (int i = 0; i < settings.DataBuilders.Count; i++)
        {
            IDataBuilder builder = settings.GetDataBuilder(i);
            if (builder.CanBuildData<AddressablesPlayModeBuildResult>() && builder.GetType() == typeof(BuildScriptPackedPlayMode))
            {
                index = i;
                break;
            }
        }
        if (settings.ActivePlayModeDataBuilderIndex != index && index > 0)
        {
            settings.ActivePlayModeDataBuilderIndex = index;
            EditorUtility.SetDirty(settings);
            AssetDatabase.Refresh();
        }
    }
    public static void BuildAddrressable(bool rebuild = false, bool update = true)
    {
        if (rebuild)
        {
            ReBuildBundle();
        }
        else if (update)
        {
            BuildUpdateBundle();
        }
    }
    /// <summary>
    /// 删除服务器包所在文件夹,重新打包的时候清除掉
    /// </summary>
    [MenuItem("SLGTools / JJAddressables / DeleteRemotePathFiles")]
    public static void ClearRemoteBuildPath()
    {
        var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
        string remotepath = settings.profileSettings.EvaluateString(settings.activeProfileId, RemoteRootPath);
        if (Directory.Exists(remotepath))
        {
            Directory.Delete(remotepath, true);
        }
    }
    #region 导表相关
    /// <summary>
    /// 导出初始分组设置，打更新包后会打乱分组
    /// </summary>
    [MenuItem("SLGTools / JJAddressables / ExportGroups")]
    public static void ExportAllAASGroups()
    {
        JJAASBuildScript.Group2Json();
        Debug.Log("导出完成");
    }
    public static string concatLabel(HashSet<string> vs)
    {
        StringBuilder builder = new StringBuilder();
        var element = vs.GetEnumerator();
        while (element.MoveNext())
        {
            builder.Append(element.Current);
            builder.Append(",");
        }
        if (builder.Length > 0)
        {
            builder.Remove(builder.Length - 1, 1);
        }
        return builder.ToString();
    }
    #endregion
    [MenuItem("SLGTools/JJAddressables/AddToPreLoad")]
    public static void Exporttxt()
    {
       /*var objs=Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        string excelPath = "../Res/Tables/打包配置表.xlsm";
        FileInfo file = new FileInfo(excelPath);
        StringBuilder builder = new StringBuilder();
        ExcelWorksheet sheet = null; int row = 0;
        ExcelPackage package = null;
        if (file.Exists)
        {
            package = new ExcelPackage(file);
            sheet = package?.Workbook.Worksheets[3];
        }
        string excludeObjs = "";
        if (objs != null)
        {
            foreach (var item in objs)
            {
                string assetPath = AssetDatabase.GetAssetPath(item);
                if (File.GetAttributes(assetPath) == FileAttributes.Directory)
                    continue;
                if (TableTools.Tables.PreLoadRes.GetLineById(assetPath.Replace("Assets/Resource_MS/", "")) != null)
                {
                    excludeObjs += assetPath+",";
                    continue;
                }
                builder.AppendLine($"{assetPath.Replace("Assets/Resource_MS/", "")}\t" +
                    $"{item.GetType().FullName}\t{item.GetType().Assembly.GetName().Name}\t\t\t");
                //builder.Append(AssetDatabase.GetAssetPath(item).Replace("Assets/Resource_MS/",""));
                //builder.Append("\n");
                if (sheet != null)
                {
                    row = sheet.Dimension.End.Row + 1;
                    sheet.Cells[row, 1].Value = assetPath.Replace("Assets/Resource_MS/", "");
                    sheet.Cells[row, 2].Value = item.GetType().FullName;
                    sheet.Cells[row, 3].Value = item.GetType().Assembly.GetName().Name;
                    sheet.Cells[row, 4].Value =default;
                    sheet.Cells[row, 5].Value =-1;
                    sheet.Cells[row, 6].Value = default;
                    sheet.Cells[row, 1].AutoFitColumns();
                    sheet.Cells[row, 2].AutoFitColumns();
                    sheet.Cells[row, 3].AutoFitColumns();
                    sheet.Cells[row, 4].AutoFitColumns();
                    sheet.Cells[row, 5].AutoFitColumns();
                    sheet.Cells[row, 6].AutoFitColumns();
                }
            }
        }
        package?.Save();
        using (FileStream stream=new FileStream("Assets/Resource_MS/DataTables/PreLoadRes.txt", FileMode.Append))
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.Write("\n"+builder.ToString());
            writer.Flush();
            writer.Close();
        }
        if (!string.IsNullOrEmpty(excludeObjs))
        {
            Debug.LogError($"{excludeObjs} 已经包含在Preload表里了,");
        }
        Debug.Log("Add2Preload Finished");
        if (sheet != null)
        {
            DataTable.ExcelImporter importer = new DataTable.ExcelImporter();
            importer.GeneratePreloadCSharp(sheet, "PreLoadRes");
            importer.GeneratePreLoadResSo("PreLoadRes", sheet); 
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        package?.Dispose();*/
    }
    [MenuItem(JJAddressableTools+ "UnBundleAllGroup")]
    public static void UnBundleAllGroup()
    {
        var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        for (int i = 0; i < setting.groups.Count; i++)
        {
            setting.groups[i].GetSchema<BundledAssetGroupSchema>().IncludeInBuild = false;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

