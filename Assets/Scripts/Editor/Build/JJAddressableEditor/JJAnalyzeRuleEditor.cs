using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class JJAnalyzeRuleEditor : AnalyzeRule
{
    public override bool CanFix
    {
        get
        {
            return base.CanFix;
        }

        set
        {
            base.CanFix = value;
        }
    }

    public override string ruleName
    {
        get
        {
            return base.ruleName;
        }
    }

    public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
    {
        List<AnalyzeResult> res = new List<AnalyzeResult>();
        var groups = settings.groups;
        foreach (var group in groups)
        {
            if (group.HasSchema<PlayerDataGroupSchema>())
                continue;
            var entries = group.entries;
            foreach (var entry in entries)
            {
                //entry.
                //操作entry 建立条件
                if (true)
                {
                    res.Add(new AnalyzeResult { resultName = group.name, severity = UnityEditor.MessageType.Error });
                }
            }
        }
        return res;
    }

    public override void FixIssues(AddressableAssetSettings settings)
    {
        base.FixIssues(settings);
    }

    public override void ClearAnalysis()
    {
        base.ClearAnalysis();
    }
}
[InitializeOnLoad]
class MyAnalyze
{
    static MyAnalyze()
    {
        AnalyzeSystem.RegisterNewRule<JJAnalyzeRuleEditor>();
    }
}
