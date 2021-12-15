using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using System.Text;
using System.Linq;

public class JJAASBuildScript
{
    static List<JJGroup> jJGroups = new List<JJGroup>();
    public const string JsonPath = "Assets/Scripts/Editor/Build/JJAddressableEditor/GroupJson.json";
    #region 导出分组设置到json，再修改部分
    public static void Group2Json()
    {
        jJGroups.Clear();
         var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        for (int i = 0; i < setting.groups.Count; i++)
        {
            var group = setting.groups[i];
            if (group.IsDefaultGroup())
            {
                continue;
            }
            BundledAssetGroupSchema bundledSchema = group.GetSchema<BundledAssetGroupSchema>();
            ContentUpdateGroupSchema updateSchema = group.GetSchema<ContentUpdateGroupSchema>();
            JJGroup jjgroup = new JJGroup()
            {
                GroupName = group.Name,
                isStatic = updateSchema.StaticContent,
                isLocal = bundledSchema.LoadPath.GetName(setting) == AddressableAssetSettings.kLocalLoadPath
            ,
                BundleMode = (int)bundledSchema.BundleMode
            };
            foreach (var item in group.entries)
            {
                jjgroup.entries.Add(new JJEntries() { path = item.AssetPath, Address = item.address, Lable = JJAddressableBuildEditor.concatLabel(item.labels), extension = "" });
            }
            jJGroups.Add(jjgroup);

        }
        using (FileStream stream = new FileStream(JsonPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            StreamWriter writer = new StreamWriter(stream,System.Text.Encoding.UTF8);
            writer.Write(JsonMapper.ToJson(jJGroups));
            writer.Flush();
            writer.Close();
        }
        jJGroups.Clear();
    }
    #endregion
    #region 从json表重新设置分组信息，重新打包调用
    public static void GroupsFromJson()
    {
        StreamReader reader = new StreamReader(JsonPath,System.Text.Encoding.UTF8);
        var json=reader.ReadToEnd();
        jJGroups = JsonMapper.ToObject<List<JJGroup>>(json);
        var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        for (int i = 0; i < setting.groups.Count; i++)
        {
            if (setting.groups[i].IsDefaultGroup())
                continue;
            JJAddressableBuildEditor.ClearGroupEntries(setting.groups[i]);
        }
        for (int i = 0; i < jJGroups.Count; i++)
        {
            AutoSetGroupInfo(jJGroups[i]);
        }
        reader.Dispose();
        reader.Close();
        jJGroups.Clear();

        JJAddressableBuildEditor.DeleteNoEntriesGroup();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.LogError("GroupsFromJson Done");
    }
    public static void AutoSetGroupInfo(JJGroup jjgroup)
    {
        var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        AddressableAssetGroup group = setting.FindGroup(jjgroup.GroupName);
        if (group == null)
        {
            group = setting.CreateGroup(jjgroup.GroupName, false, false, false, new List<AddressableAssetGroupSchema>() { setting.DefaultGroup.Schemas[0], setting.DefaultGroup.Schemas[1] });
        }
        BundledAssetGroupSchema bundleSchema = group.GetSchema<BundledAssetGroupSchema>();
        ContentUpdateGroupSchema UpdateSchema = group.GetSchema<ContentUpdateGroupSchema>();
        if (jjgroup.isLocal||!setting.BuildRemoteCatalog)
        {
            bundleSchema.BuildPath.SetVariableByName(group.Settings,AddressableAssetSettings.kLocalBuildPath);
            bundleSchema.LoadPath.SetVariableByName(group.Settings,AddressableAssetSettings.kLocalLoadPath);
        }
        else
        {
            bundleSchema.BuildPath.SetVariableByName(group.Settings,AddressableAssetSettings.kRemoteBuildPath);
            bundleSchema.LoadPath.SetVariableByName(group.Settings,AddressableAssetSettings.kRemoteLoadPath);
        }
        bundleSchema.Compression = BundledAssetGroupSchema.BundleCompressionMode.LZ4;
        bundleSchema.IncludeInBuild = true;
        bundleSchema.ForceUniqueProvider = false;
        bundleSchema.UseAssetBundleCache = true;
        bundleSchema.BundleMode =(BundledAssetGroupSchema.BundlePackingMode)jjgroup.BundleMode;
        //做增量更新设置
        //本地的bundle不带hash，避免更新之后找不到bundle
        bundleSchema.BundleNaming =BundledAssetGroupSchema.BundleNamingStyle.NoHash;
        bundleSchema.UseAssetBundleCrc = false;
        bundleSchema.AssetBundledCacheClearBehavior = BundledAssetGroupSchema.CacheClearBehavior.ClearWhenWhenNewVersionLoaded;
        UpdateSchema.StaticContent = jjgroup.isStatic;
        for (int i = 0; i < jjgroup.entries.Count; i++)
        {
            SetGroupEntries(group, jjgroup.entries[i]);
        }
    }

    static bool isBuildInScene( string assetName)
    {
        var scenes = EditorBuildSettings.scenes;
        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i].path == assetName && scenes[i].enabled)
                return true;
        }
        return false;
    }

    public static void SetGroupEntries(AddressableAssetGroup group,JJEntries jEntries,bool ignoreExits=false)
    {
        var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        var entryPath = setting.profileSettings.EvaluateString(setting.activeProfileId, jEntries.path);

        if (isBuildInScene(entryPath))
            return;

        if (!IsValidPath(entryPath))
            return;

        var guid = AssetDatabase.AssetPathToGUID(entryPath);

        var lables = jEntries.Lable.Split(',',';',':');

        if (!string.IsNullOrEmpty(jEntries.extension))
        {
            if (jEntries.extension.StartsWith("*."))
            {
                //指定的的子文件都是一个单独的entry
                if (AssetDatabase.IsValidFolder(entryPath))
                {
                    var subpaths = Directory.GetFiles(entryPath, jEntries.extension, SearchOption.AllDirectories);
                    for (int i = 0; i < subpaths.Length; i++)
                    {
                        var subpath = subpaths[i].Replace("\\", "/");
                        var subguid = AssetDatabase.AssetPathToGUID(subpath);
                        if (!IsValidPath(subpath) || setting.FindAssetEntry(subguid) != null)//对于扩展的路径如果已经被加入到别的组，则不处理
                        {
                            continue;
                        }
                        SetGroupEntries(group, new JJEntries() { path = subpath, Lable = jEntries.Lable }, ignoreExits);
                    }
                }
                return;
            }
            else
            {
                if(jEntries.extension=="f")//所有子文件夹都是一个entry
                {
                    var subfolders = AssetDatabase.GetSubFolders(entryPath);
                    for (int i = 0; i < subfolders.Length; i++)
                    {
                        var subguid = AssetDatabase.AssetPathToGUID(subfolders[i]);
                        if (!IsValidPath(subfolders[i]) || setting.FindAssetEntry(subguid) != null)//对于扩展的路径如果已经被加入到别的组，则不处理
                        {
                            continue;
                        }
                        SetGroupEntries(group, new JJEntries() { path = subfolders[i],Lable=jEntries.Lable}, ignoreExits);
                    }
                }
            }
        }

        var entries = setting.FindAssetEntry(guid);
        if (entries != null && ignoreExits)///对于已经存在的entry，如果设置为true，则不移动也不关心
            return;
        entries=setting.CreateOrMoveEntry(guid, group);
        entries.labels.Clear();
        for (int i = 0; i < lables.Length; i++)
        {
            entries.SetLabel(lables[i], true, true);
        }
        if (!string.IsNullOrEmpty(jEntries.Address))
        {
            entries.SetAddress(setting.profileSettings.EvaluateString(setting.activeProfileId, jEntries.Address));
        }
    }
    #endregion

    #region 打更新包由此检查更新文件，打更新bundle
    /// <summary>
    /// 目的是把未添加的条目添加到分组中，此时创建的分组都是为了更新的
    /// </summary>
    public static void FixGroupAfterMerge()
    {
        StreamReader reader = new StreamReader(JsonPath, System.Text.Encoding.UTF8);
        var json = reader.ReadToEnd();
        jJGroups.Clear();
        jJGroups = JsonMapper.ToObject<List<JJGroup>>(json);
        reader.Close();
        var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        for (int i = 0; i < jJGroups.Count; i++)
        {
            var jjgroup = jJGroups[i];
            var group = CreateUpdateContentGroup(jjgroup);//这样的分组在之后的更新之中，也是走增量的方式
            for (int j = 0; j < jjgroup.entries.Count; j++)
            {
                SetGroupEntries(group, jjgroup.entries[j], true);
            }
        }
    }
    public static AddressableAssetGroup CreateUpdateContentGroup(JJGroup jjGroup,bool StaticContent=true)
    {
        var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        AddressableAssetGroup group = setting.FindGroup(jjGroup.GroupName);
        if (group == null)
        {
            group = setting.CreateGroup(jjGroup.GroupName, false, false, false, new List<AddressableAssetGroupSchema>() { setting.DefaultGroup.Schemas[0],
        setting.DefaultGroup.Schemas[1]});
            BundledAssetGroupSchema bundleSchema = group.GetSchema<BundledAssetGroupSchema>();
            ContentUpdateGroupSchema updateSchema =group.GetSchema<ContentUpdateGroupSchema>();
            updateSchema.StaticContent = StaticContent;
            bundleSchema.Compression = BundledAssetGroupSchema.BundleCompressionMode.LZMA;//暂时设置，视频、音频的压缩方式有待测试
            bundleSchema.BundleMode = (BundledAssetGroupSchema.BundlePackingMode)jjGroup.BundleMode;
            bundleSchema.BuildPath.SetVariableByName(setting, AddressableAssetSettings.kRemoteBuildPath);
            bundleSchema.LoadPath.SetVariableByName(setting, AddressableAssetSettings.kRemoteLoadPath);
            bundleSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;
            bundleSchema.AssetBundledCacheClearBehavior = BundledAssetGroupSchema.CacheClearBehavior.ClearWhenWhenNewVersionLoaded;
        }
        return group;
    }
    public static void CheckForUpdateContent()
    {
        Debug.Log("开始检测是否存在包内文件有修改");
        var path = ContentUpdateScript.GetContentStateDataPath(false);//不打开browser
        if (string.IsNullOrEmpty(path))
            Debug.LogWarning("No path specified for Content State Data file.");
        else if (!File.Exists(path))
            Debug.LogWarningFormat("No Content State Data file exists at path: {0}");
        else
            PrepareForContentUpdate(AddressableAssetSettingsDefaultObject.GetSettings(true), path);
    }
    public static void UpdateAPreviousBuild()
    {
        var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
        var path = ContentUpdateScript.GetContentStateDataPath(false);
        if (!string.IsNullOrEmpty(path))
            ContentUpdateScript.BuildContentUpdate(setting, path);

        Debug.Log("打包完成" + setting.RemoteCatalogBuildPath.GetValue(setting));
    }

    public static bool PrepareForContentUpdate(AddressableAssetSettings settings, string path, string updateGroupName = "ContentUpdate")
    {
        Dictionary<string, JJGroup> groupPairs = jJGroups.ToDictionary((group)=> { return group.GroupName; });
        var modifiedEntries = ContentUpdateScript.GatherModifiedEntries(settings, path);
        if (modifiedEntries.Count == 0)
        {
            Debug.Log("未检测到包内Group目录需要更改");
            return false;
        }
        
        Debug.Log("检测到包内Group目录需要修改");

        StringBuilder sbuider = new StringBuilder();
        sbuider.AppendLine("需要更新的包内文件:");

        var dicUpdateGroups = new Dictionary<string, List<AddressableAssetEntry>>();
        var newGroupPackTogether = new Dictionary<string, bool>();
        string newUpdateGroupName = updateGroupName;//$"{updateGroupName}-{System.DateTime.Now:u}";
        var newContentUpdateGroup = settings.FindGroup(newUpdateGroupName)?? CreateUpdateContentGroup(new JJGroup() 
            { GroupName=newUpdateGroupName,BundleMode=(int)BundledAssetGroupSchema.BundlePackingMode .PackSeparately},false);

        List<AddressableAssetEntry> entries =modifiedEntries.FindAll((entry)=> { 
            if (!IsValidEntry(entry)) return false;
            var m_Entry = entry;
            sbuider.AppendLine(entry.AssetPath);
            if (groupPairs.TryGetValue(entry.parentGroup.Name, out JJGroup group))
            {
                var u_Type = FindEntryUpdateType(group.entries, entry.address);
                if (u_Type == JJEntries.EntryUpdateType.U_WithParentEntry)
                {
                    entry = FindRootEntry(entry);
                }
            }
            return true;
        });
        settings.MoveEntries(entries, newContentUpdateGroup, false, false);

        return true;
    }
    public static JJEntries.EntryUpdateType FindEntryUpdateType(List<JJEntries> jJEntries,string address)
    {
        JJEntries.EntryUpdateType type = JJEntries.EntryUpdateType.U_Separately;
        int index = -1;
        jJEntries.ForEach((entry) => {
            int idx = address.IndexOf(entry.Address);
            if (idx > index)
            {
                index = idx;
                type = entry.updateType;
            }
        });
        return type;
    }
    public static AddressableAssetEntry FindRootEntry(AddressableAssetEntry entry)
    {
        while (entry.ParentEntry!=null)
        {
            entry = entry.ParentEntry;
        }
        return entry;
    }
    /// <summary>
    /// 不支持的文件类型
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static bool IsValidEntry(AddressableAssetEntry entry)
    {
        if (entry == null || string.IsNullOrEmpty(entry.AssetPath))
            return false;
        var assetType= AssetDatabase.GetMainAssetTypeAtPath(entry.AssetPath);
        if((assetType == null || assetType == typeof(DefaultAsset)) && !AssetDatabase.IsValidFolder(entry.AssetPath))
        {
            return false;
        }
        return true;
    }
    #endregion
    public static bool IsValidPath(string path)
    {
        if (path.EndsWith(".meta"))
            return false;
        var guid = AssetDatabase.AssetPathToGUID(path);
        if (!string.IsNullOrEmpty(guid) && (Directory.Exists(path) || File.Exists(path)))
        {
            return true;
        }
        else
        {
            Debug.LogWarning($"assetPath -{path} 不存在 请检查路径");
            return false;
        }
    }
    [MenuItem(JJAddressableBuildEditor.JJAddressableTools + "OverrideGroupJson")]
    public static void OverrideGroupJson()
    {
        StreamReader reader = new StreamReader(JsonPath, System.Text.Encoding.UTF8);
        var json = reader.ReadToEnd();
        List<JJGroup> groups = JsonMapper.ToObject<List<JJGroup>>(json);
        
        reader.Close();
        using (FileStream stream = new FileStream(JsonPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
            writer.Write(JsonMapper.ToJson(groups));
            writer.Flush();
            writer.Close();
        }
    }
}


public class JJGroup
{
    public string GroupName;
    public List<JJEntries> entries=new List<JJEntries>();
    public bool isLocal = true;
    public bool isStatic = true;
    public int BundleMode;
}
public class JJEntries
{
    public enum EntryUpdateType
    {
        U_Separately=0,
        U_WithParentEntry=1,
    }
    public string path;
    public string Address;
    public string Lable;
    public string extension = "";
    public EntryUpdateType updateType=EntryUpdateType.U_Separately;//默认自身拆出一个entry

}
