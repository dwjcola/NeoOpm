using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets.GUI;
using UnityEditor.AddressableAssets.Build;

public class JJPathAddressIsPath : AnalyzeRule
{
    public override bool CanFix
    {
        get
        {
            return true;
        }
    }

    public override string ruleName
    {
        get
        {
            return "校验资源名称/路径 是否匹配";
        }
    }
    [SerializeField]
    List<AddressableAssetEntry> misNameEntries = new List<AddressableAssetEntry>();
    public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
    {
        List<AnalyzeResult> results = new List<AnalyzeResult>();
        foreach (var group in settings.groups)
        {
            if (group.HasSchema<PlayerDataGroupSchema>())
                continue;

            foreach (var e in group.entries)
            {
                if (e.address.Contains("Assets") && e.address.Contains("/") && e.address != e.AssetPath)
                {
                    misNameEntries.Add(e);
                    results.Add(new AnalyzeResult { resultName = group.Name + kDelimiter + e.address, severity = MessageType.Error });
                }
            }
        }

        if (results.Count == 0)
        {
            results.Add(new AnalyzeResult { resultName = "No issues found", severity = MessageType.Error });
        }
        return results;
    }

    public override void FixIssues(AddressableAssetSettings settings)
    {
        foreach (var e in misNameEntries)
        {
            e.address = e.AssetPath;
        }

        if (misNameEntries.Count > 0)
            misNameEntries.Clear();
    }

    public override void ClearAnalysis()
    {
        if (misNameEntries.Count > 0)
            misNameEntries.Clear();
    }
}

[InitializeOnLoad]
class RegisterMyPathAddressIsPath
{
    static RegisterMyPathAddressIsPath()
    {
        AnalyzeSystem.RegisterNewRule<JJPathAddressIsPath>();
    }
}
