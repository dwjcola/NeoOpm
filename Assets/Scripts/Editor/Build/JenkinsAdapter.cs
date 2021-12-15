using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using LitJson;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Build;
using UnityEngine.Rendering;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;


public class JenkinsAdapter
{
    public static string PlateformName
    {
        get
        {
            //在这里分析shell传入的参数， 还记得上面我们说的哪个 project-$1 这个参数吗？
            //这里遍历所有参数，找到 project开头的参数， 然后把-符号 后面的字符串返回，
            //这个字符串就是 91 了
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                Debug.Log("GetCommandLineArgs:" + arg);
                if (arg.StartsWith("plateform"))
                {
                    return arg.Split("-"[0])[1];
                }
            }

            return "test";
        }
    }


    public static string VersionName
    {
        get
        {
            //在这里分析shell传入的参数， 还记得上面我们说的哪个 project-$1 这个参数吗？
            //这里遍历所有参数，找到 project开头的参数， 然后把-符号 后面的字符串返回，
            //这个字符串就是 91 了
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                Debug.Log("GetCommandLineArgs:" + arg);
                if (arg.StartsWith("version"))
                {
                    return arg.Split("-"[0])[1];
                }
            }

            return "0000";
        }
    }

    public static string Version
    {
        get
        {
            //在这里分析shell传入的参数， 还记得上面我们说的哪个 project-$1 这个参数吗？
            //这里遍历所有参数，找到 project开头的参数， 然后把-符号 后面的字符串返回，
            //这个字符串就是 91 了
            //            foreach (string arg in System.Environment.GetCommandLineArgs())
            var cmdLines = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-version" && i + 1 < cmdLines.Length)
                {
                    return cmdLines[i + 1];
                }
            }

            return "000";
        }
    }

    public static string BuildPath
    {
        get
        {
            var cmdLines = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-buildPath" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("buildPath" + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }

            return "./xcodeproj";
        }
    }

    public static bool _isDebug;
    public static bool IsDebug
    {
        get
        {
            var cmdLines = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-debug" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("debugMode" + cmdLines[i + 1]);
                    int op = 0;
                    if(int.TryParse(cmdLines[i + 1], out op))
                    {
                        switch (op)
                        {
                            case 0:buildOptions = BuildOptions.None;break;
                            case 1:buildOptions = BuildOptions.Development |BuildOptions.ConnectWithProfiler;break;
                        }
                    }
                    return op!=0;
                }
            }
            
            return _isDebug;
        }
    }
   public static  BuildOptions buildOptions;

    enum ValueResult
    {
        Disable,
        True,
        False,
    }



    public static string BuildNum
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-buildnum" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("buildnum" + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }

            return "0";
        }
    }
    public static bool _rebuildAB;
    public static bool RebuildAB
    {
        get
        {
            var cmdLines = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-rebuildAB" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("rebuildAB" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }

            return _rebuildAB;
        }
    }
    public static bool _updateAb;
    public static bool updateAB
    {
        get
        {
            var cmdLines = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-updateAB" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("updateAB" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }

            return _updateAb;
        }
    }

    public static bool MultiThreading
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-multiThreading" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("enable multi-threading" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return true;
        }
    }

    public static bool IsBatching
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-isBatching" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("enable static and dynamic batching" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return true;
        }
    }

    public static bool IsPc
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-ispc" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("is pc" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return false;
        }
    }
    public static bool _mutelog;
    public static bool MuteLog
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-mutelog" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("mutelog" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return _mutelog;
        }
    }
    public static bool _opengm;
    public static bool OpenGm
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-opengm" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("opengm " + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return _opengm;
        }
    }

    public static int BundleVersionCode
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-bundleCode" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("bundleCode" + cmdLines[i + 1]);
                    return int.Parse(cmdLines[i + 1]);
                }
            }
            return 2;
        }
    }

    public static bool WITH_QQ_WECHAT
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-withQq" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("WITH_QQ_WECHAT" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return false;
        }
    }

    public static string Channel
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-channel" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("CHANNEL" + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }
            return "Lobby1";
        }
    }
    public static string BranchName
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-branchname" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("branchName" + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }
            return "Dev";
        }
    }


    public static string PackageName
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-packageName" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("package name " + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }
            return "cn.jj.KOSea";
        }
    }

    public static string ProductName
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-productname" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("product name " + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }
            return "七海之王";
        }
    }

    public static bool IsTestServer
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-testServer" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("testserver is" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return false;
        }
    }

    public static string SignKey
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-signKey" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("keystore is" + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }
            return "cq";
        }
    }
    private static bool _hotfix;
    public static bool IsHotfix
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-hotfix" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("hotfix is" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return _hotfix;
        }
    }
    public static bool _il2cpp;
    public static bool ILCpp
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-ilcpp" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("ilcpp is" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return _il2cpp;
        }
    }

    public static string IosPp
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-iospp" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("iospp is" + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }
            return "ad-hoc";
        }
    }
    public static string _architecture = "All";
    public static string Architecture
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-architecture" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("architecture is" + cmdLines[i + 1]);
                    return cmdLines[i + 1];
                }
            }
            return _architecture;
        }
    }
    public static bool _autoGraphic = true;
    public static bool AutoGraphic
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-autoGraphic" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("AutoGraphic is" + cmdLines[i + 1]);
                    return cmdLines[i + 1]=="true";
                }
            }
            return _autoGraphic;
        }
    }
    public static List<GraphicsDeviceType> _graphicAPIs=new List<GraphicsDeviceType>();
    public static GraphicsDeviceType[] GraphicAPIs
    {
        get
        {
            _graphicAPIs.Clear();
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-graphicsAPIs" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("AutoGraphic is" + cmdLines[i + 1]);
                    string[] apis = cmdLines[i + 1].Split(',');
                    for (int j = 0; j < apis.Length; j++)
                    {
                        _graphicAPIs.Add((GraphicsDeviceType)Enum.Parse(typeof(GraphicsDeviceType), apis[j]));
                    }
                }
            }
            return _graphicAPIs.ToArray();
        }
    }
    static string GetGraphicApisString(BuildTarget target)
    {
        if (PlayerSettings.GetUseDefaultGraphicsAPIs(target))
        {
            return "AutoGraphic";
        }
        var apis=PlayerSettings.GetGraphicsAPIs(target);
        //for (int i = 0; i < apis.Length; i++)
        //{
        //    api += apis.ToString()+"-";
        //}
        return string.Concat(apis);
    }
    public static BuildTarget buildTarget = BuildTarget.Android;
    public static BuildTarget BuildTarget
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-buildTarget" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("buildTarget is" + cmdLines[i + 1]);
                    return (BuildTarget)Enum.Parse(typeof(BuildTarget), cmdLines[i + 1]);
                }
            }
            return buildTarget;
        }
    }

    const string enterprise = "enterprise";
    const string ad_hoc = "ad-hoc";
    const string app_store = "app-store";
    const string developer = "development";
    public static string IosManualProvisioningProfileId
    {
        get
        {
            const string adHocProvisioningProfileId = "c16422ff-88f2-4aaf-9c08-4a29698ac572";
            const string inHouseProvisioningProfileId = "afba90b8-8f91-4d54-b8bd-a546d78a3375";
            const string appStoreProvisioningProfileId = "";
            
            const string developerProvisioningProfileId = "a1a05b8c-fdd3-4bbc-80c7-fd2d6312be0d";
            string ret = string.Empty;
            switch (IosPp)
            {
                case ad_hoc:
                    ret = adHocProvisioningProfileId;
                    break;
                case enterprise:
                    ret = inHouseProvisioningProfileId;
                    break;
                case app_store:
                    ret = appStoreProvisioningProfileId;
                    break;
                case developer:
                    ret = developerProvisioningProfileId;
                    break;
                default:
                    Debug.LogError("IosPP error");
                    break;
            }
            Debug.LogFormat("Provisioning Profile Id {0}", ret);
            return ret;
        }
    }

    public static string TeamId
    {
        get
        {
            const string adHocTeamId = "6LRF78BQ67";
            const string inHouseTeamId = "6LRF78BQ67";
            const string appStoreTeamId = "6LRF78BQ67";
            const string developerTeamId = "6LRF78BQ67";
            string ret = string.Empty;
            switch (IosPp)
            {
                case ad_hoc:
                    ret = adHocTeamId;
                    break;
                case enterprise:
                    ret = inHouseTeamId;
                    break;
                case app_store:
                    ret = appStoreTeamId;
                    break;
                case developer:
                    ret = developerTeamId;
                    break;
                default:
                    Debug.LogError("TeamId error");
                    break;
            }
            Debug.LogFormat("TemaId {0}", ret);
            return ret;
        }
    }

    public static string Ios_Sign_Identity
    {
        get
        {
            return IosPp==developer? "iPhone Developer":"iPhone Distribution";
        }
    }

    public static bool IsIosDelivery
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-delivery" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("delivery is" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return false;
        }
    }

    public static bool IsSmallPack
    {
        get
        {
            var cmdLines = Environment.GetCommandLineArgs();
            for (int i = 0; i < cmdLines.Length; i++)
            {
                if (cmdLines[i] == "-smallpack" && i + 1 < cmdLines.Length)
                {
                    Debug.Log("smallpack is" + cmdLines[i + 1]);
                    return cmdLines[i + 1] == "true";
                }
            }
            return false;
        }
    }
   static string[] excludeFolder = new string[] { "Assets/ScriptsEditor", "Assets/Art/Beautify" };
    private static void ExcludeUnUseFolder()
    {
        for (int i = 0; i < excludeFolder.Length; i++)
        {
            if (Directory.Exists(excludeFolder[i]))
            {
                Directory.Delete(excludeFolder[i],true);
            }
        }
        AssetDatabase.Refresh();
    } 
    private static void AddDefineSymbol(BuildTargetGroup group, string symbol)
    {
        string currentSymbol = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        var newSymbol = string.Format("{0};{1}", currentSymbol, symbol);
        Debug.Log("Script define " + newSymbol);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, newSymbol);
    }

    private static void ResetDefineSymbol(BuildTargetGroup group)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, "ODIN_INSPECTOR");
    }

    [MenuItem("BuildTool/Jenkins/AndroidBuild")]
    public static void ShowAndroidWindow()
    {
        SlgBuildWindow.Init();
    }
    public static void BuildApp()
    {
        switch (BuildTarget)
        {
            case BuildTarget.Android:
                AndroidBuild();
                break;
            case BuildTarget.iOS:
                //TODO
                IosBuild();
                break;
        }

    } 
    /// <summary>
    /// 刷新版本号
    /// </summary>
    //[MenuItem(JJAddressableBuildEditor.JJAddressableTools+"TestGetVersion")]
    //public static void TestGetVersion()
    //{
        //GetVersion();
    //}
    public static string GetVersion(string branchName="Dev")
    {
        TextAsset text=AssetDatabase.LoadAssetAtPath<TextAsset>(ProHA.AssetUtility.ResourceRoot+"Configs/BuildInfo.txt");
        var buildInfoJson= JsonMapper.ToObject(text.text);
        string version =(string)buildInfoJson["GameVersion"];
        version = string.IsNullOrEmpty(version) ? "1.0.0" : version;
        string[] verSlice=version.Split('.');
        string newMiddleVer = verSlice[1];
        string ver = $"{verSlice[0]}.{verSlice[1]}";
        string versionPath = Path.Combine(Application.dataPath.Replace("\\","/"),"../../../BuildVer.ini");

        int number = 0;
        JsonData buildVerJson=new JsonData();
        if (File.Exists(versionPath))
        {
            var verStr = File.ReadAllText(versionPath);
            buildVerJson = JsonMapper.ToObject(verStr);
            try
            {
                verStr =(string)buildVerJson[branchName];
            }
            catch
            {
            }
            verSlice = verStr.Split( '.');
            string oldMidVer = verSlice[1];
            string buildNum = verSlice[2];
            int.TryParse(buildNum, out number);
            if (oldMidVer != newMiddleVer)
                number = 0;
        }
        number++;
        string appVersion = $"{ver}.{number}";
        buildInfoJson["GameVersion"] = appVersion;
        buildVerJson[branchName]= appVersion;
        SaveFile(versionPath, JsonMapper.ToJson(buildVerJson));
        SaveFile(ProHA.AssetUtility.ResourceRoot + "Configs/BuildInfo.txt", JsonMapper.ToJson(buildInfoJson));
        AssetDatabase.SaveAssets();
        return appVersion;
    }
    

    public static void AndroidBuild()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        DeleteXluaGen();
        ExcludeUnUseFolder();///删除掉不需要打包进去的文件夹
        JJAddressableBuildEditor.ChangeActivePlayMode();
        JJAddressableBuildEditor.BuildAddrressable(RebuildAB, updateAB);

        if (updateAB)//打更新bundle，不需要再打包
        {
            return;
        }
        //if (!AssetBundleManager.SimulateAssetBundleInEditor)
        //{
        //    Debug.LogWarning("必须在模拟模式下执行");
        //    return;
        //}
        //		ResetFbxNormalAndTagents("Resources_MS/Scene");
        //		ResetFbxNormalAndTagents("OldRes/Resources_MS/Scene");

        //string DeletPath = Path.Combine(Application.dataPath ,"ScriptsEditor");

        string ver = GetVersion(BranchName);
        PlayerSettings.bundleVersion = ver;
        //string ver = PlayerSettings.bundleVersion; //BuildScript.GetVerion();

        //        if (IsPc)
        //        {
        //            AddDefineSymbol(BuildTargetGroup.Android, "FROMPC");    
        //        }
        if (IsHotfix)
        {
            AddDefineSymbol(BuildTargetGroup.Android, "HOTFIX_USED");
        }
        //        if (WITH_QQ_WECHAT)
        //        {
        //            AddDefineSymbol(BuildTargetGroup.Android, "WITH_QQ_WECHAT");
        //        }
        //        if (IsToutiao(Channel))
        //        {
        //            AddDefineSymbol(BuildTargetGroup.Android, "TouTiao");
        //        }
       
        if (IsDebug)
        {
            //            DeleteHotUpdateJar();
        }

        //AutoIconSet.SetAndroidAdaptive();
        //AutoIconSet.SetAndroidLegacy();
        //AutoIconSet.SetAndroidRound();
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, AutoGraphic);
        if (!AutoGraphic)
        {
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android,GraphicAPIs);
        }

        //SetUrl();

        SetProductName();
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, PackageName);
        //PlayerSettings.bundleVersion = string.Format("{0}.{1}", ver, BuildNum);
       // PlayerSettings.Android.bundleVersionCode = BuildScript.GetBundleId();
        //PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android,ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

        PlayerSettings.Android.targetArchitectures =(AndroidArchitecture)Enum.Parse(typeof(AndroidArchitecture), Architecture,true);
       // PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Disabled);
        PlayerSettings.Android.useCustomKeystore = false;
        //PlayerSettings.Android.keystoreName = Path.Combine(Environment.CurrentDirectory, "KeyStore/chunqiu.keystore");
        //PlayerSettings.Android.keystorePass = "xinghuo";
        //PlayerSettings.Android.keyaliasName = "cq";
        //PlayerSettings.Android.keyaliasPass = "xinghuo";
        PlayerSettings.MTRendering = MultiThreading;
        if (ILCpp)
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        }
        else
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        }

        if (!SetBatchingProperty()) return;

        if (OpenGm)
        {
            AddDefineSymbol(BuildTargetGroup.Android, "OPENGM");
        }

        if (MuteLog)
        {
            AddDefineSymbol(BuildTargetGroup.Android, "ENABLE_LOG");
        }

#if HOTFIX_USED
        XLuaRegister.CreateHotfixJson ( );
#endif
        //SerializeDemo.ToBinaryStatic();
        //BuildScript.MovePatchConfigFile();
        //BuildScript.Execute(withAb, IsSmallPack);
        //BuildScript.SaveVerFile(IsSmallPack, BuildNum);
        
        CSObjectWrapEditor.Generator.GenAll();
        BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = GetLevelsFromBuildSettings(),
            //assetBundleManifestPath = Utility.StreamingAssetPath,
            locationPathName = Path.Combine(BuildPath, string.Format("slg_android_{0}.{1}_{2}_{3}.apk", ver, BuildNum, BranchName, GetGraphicApisString(BuildTarget.Android))),
            target = BuildTarget.Android,
            options = buildOptions
        });

        ResetDefineSymbol(BuildTargetGroup.Android);
        //BuildBatClass.RevertGitPath(DeletPath);
        Debug.Log("完成apk文件生成!");
    }
    /// <summary>
    /// 打bundle更新包的接口,所有平台均适用
    /// </summary>
    public static void BuildTargetUpdateAB()
    {
        JJAddressableBuildEditor.ChangeActivePlayMode();
        JJAddressableBuildEditor.BuildContentUpdate();
    }
    // 提交文件
    static void SetIosDeliveryFile()
    {
        string DeliveryDir = "Assets/Plugins/iOS/.sdk/delivery";
        string DestDir = "Assets/Resources_MS";
        var files = Directory.GetFiles(DeliveryDir, "*.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string srcFile = file;
            string dstFile = file.Replace(DeliveryDir, DestDir);
            if (File.Exists(dstFile))
            {
                File.Delete(dstFile);
            }
            File.Copy(srcFile, dstFile, true);
        }
        AssetDatabase.Refresh();
    }

    // 猜测/u这个转义符是作为后面2个字节的
    static private byte[] FromUnicode(string str)
    {
        byte[] b = new byte[str.Length / 3];
        int j = 0;
        for (int i = 0; i < str.Length; i += 6)
        {
            string str1 = str.Substring(i + 4, 2);
            Byte a = Byte.Parse(str1, NumberStyles.HexNumber);
            b[j++] = a;
            str1 = str.Substring(i + 2, 2);
            a = Byte.Parse(str1, NumberStyles.HexNumber);
            b[j++] = a;
        }
        return b;
    }

    static void SetProductName()
    {
        Encoding unicoding = new UnicodeEncoding(false, false);
        PlayerSettings.productName = ProductName;// unicoding.GetString(FromUnicode(ProductName));
    }

    static void SetSplash(string channel)
    {
        string[] dirNames = { "GameLogo" };
        string logoDir = "Assets/Resources_MS/UI";
        string sdkDir = "Assets/Plugins/Android/.sdk";
        string fileDir = Path.Combine(sdkDir, channel);
        foreach (var dirName in dirNames)
        {
            string srcDir = Path.Combine(fileDir, dirName);
            if (!Directory.Exists(srcDir))
            {
                return;
            }
            string destDir = Path.Combine(logoDir, dirName);
            var files = Directory.GetFiles(srcDir, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string newFile = file.Replace(srcDir, destDir);
                File.Copy(file, newFile, true);
            }
        }
    }
    static void SetUrl()
    {
        string fileName ="" /*Utility.PlatformConfigFile*/;
        string dstDir = "Assets/StreamingAssets";
        string sdkDir = "Assets/Plugins/Android/.sdk";
        // string fileDir = Path.Combine(sdkDir, Channel);
        string srcFile = Path.Combine(sdkDir, fileName);
        string destFile = Path.Combine(dstDir, fileName);
        if (File.Exists(srcFile))
        {
            File.Copy(srcFile, destFile, true);
        }
        else
        {
            Debug.LogError(srcFile + "not exist!");
        }
    }

    static void SetIosUrl()
    {
        string fileName = ""/*Utility.PlatformConfigFile*/;
        string dstDir = "Assets/StreamingAssets";
        string sdkDir = "Assets/Plugins/iOS/.sdk";
        //        string fileDir = Path.Combine(sdkDir, Channel);
        string srcFile = Path.Combine(sdkDir, fileName);
        string destFile = Path.Combine(dstDir, fileName);
        if (File.Exists(srcFile))
        {
            File.Copy(srcFile, destFile, true);
        }
        else
        {
            Debug.LogError(srcFile + "not exist!");
        }
    }

    static string[] GetLevelsFromBuildSettings()
    {
        var levels = new List<string>();
        for (var i = 0; i < EditorBuildSettings.scenes.Length; ++i)
        {
            if (EditorBuildSettings.scenes[i].enabled)
                levels.Add(EditorBuildSettings.scenes[i].path);
        }

        return levels.ToArray();
    }

    private static bool SetBatchingProperty()
    {
        Assembly asm = Assembly.GetAssembly(typeof(Editor));
        Type type = asm.GetType("UnityEditor.PlayerSettings");
        if (type == null)
        {
            Debug.LogError("type of PlayerSettings not found");
            return false;
        }
        var methodInfo = type.GetMethod("SetBatchingForPlatform", BindingFlags.Static | BindingFlags.NonPublic);
        if (methodInfo == null)
        {
            Debug.LogError("no method SetBatchingForPlatform");
            return false;
        }

        int useBatching = IsBatching ? 1 : 0;
        methodInfo.Invoke(null, new object[] { BuildTarget.Android, useBatching, useBatching });
        return true;
    }


    [MenuItem("BuildTool/Jenkins/DeleteXluaGen")]
    public static void DeleteXluaGen()
    {
        string xluaDir = "Assets/XLua/Gen";
        if (Directory.Exists(xluaDir))
        {
            Directory.Delete(xluaDir, true);
        }
        AssetDatabase.Refresh();
        //        AddressableAssetSettings.BuildPlayerContent();
    }

    [MenuItem("BuildTool/Jenkins/IosBuild")]
    public static void IosBuild()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        }
        ExcludeUnUseFolder();
        DeleteXluaGen();
        JJAddressableBuildEditor.ChangeActivePlayMode();
        JJAddressableBuildEditor.BuildAddrressable(RebuildAB, updateAB);

        if (updateAB)//打更新bundle，不需要再打包
        {
            return;
        }
        

        string ver =GetVersion(BranchName)/*BuildScript.GetVerion()*/;
        PlayerSettings.bundleVersion = ver;
        //        AddDefineSymbol(BuildTargetGroup.Android, "UMENG");

        //        if (IsPc)
        //        {
        //            AddDefineSymbol(BuildTargetGroup.Android, "FROMPC");    
        //        }
        if (IsHotfix)
        {
            AddDefineSymbol(BuildTargetGroup.iOS, "HOTFIX_ENABLE");
        }
        //SetIosUrl();
        
        //Delivery:
        if (IsIosDelivery)
        {
            SetIosDeliveryFile();
        }
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, PackageName);
        PlayerSettings.bundleVersion = string.Format("{0}", ver);
        //PlayerSettings.iOS.buildNumber = BuildScript.GetBundleId().ToString();
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1);
    
        PlayerSettings.MTRendering = MultiThreading;
        PlayerSettings.iOS.allowHTTPDownload = true;
        // 这个是发布证书
        PlayerSettings.iOS.iOSManualProvisioningProfileID = IosManualProvisioningProfileId;
        PlayerSettings.iOS.appleEnableAutomaticSigning = false;
        PlayerSettings.iOS.appleDeveloperTeamID = TeamId;
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        PlayerSettings.iOS.targetOSVersionString = "11.0";
        PlayerSettings.iOS.scriptCallOptimization = ScriptCallOptimizationLevel.SlowAndSafe;
        PlayerSettings.stripEngineCode = false;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Disabled);

        if (!SetBatchingProperty()) return;

        if (OpenGm)
        {
            AddDefineSymbol(BuildTargetGroup.iOS, "OPENGM");
        }
        if (MuteLog)
        {
            AddDefineSymbol(BuildTargetGroup.iOS, "ENABLE_LOG");
        }

#if HOTFIX_USED
        XLuaRegister.CreateHotfixJson ( );
#endif
        //SerializeDemo.ToBinaryStatic();
        //BuildScript.Execute(withAb, false);
        //BuildScript.SaveVerFile(IsSmallPack, BuildNum);

        //  PlayerSettings.iOS.iOSManualProvisioningProfileID = "e775a185-82d9-467e-9e19-cdf144d3fdb6";
        CSObjectWrapEditor.Generator.GenAll();
        if (Directory.Exists("xcodeproj"))
        {
            Directory.Delete("xcodeproj", true);
        }

        BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = GetLevelsFromBuildSettings(),
            locationPathName = BuildPath,
            target = BuildTarget.iOS,
            options = IsDebug ? BuildOptions.Development | BuildOptions.AllowDebugging : BuildOptions.None
        });

        ResetDefineSymbol(BuildTargetGroup.iOS);
        Debug.Log("IOS Build done!");
    }

/// <summary>
/// BuildPipeline.BuildPlayer 打包完成会通过PostProcessBuildAttribute 通知
/// </summary>
/// <param name="target"></param>
/// <param name="pathToBuiltProjecct"></param>
#if UNITY_IOS
    [PostProcessBuildAttribute(0)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProjecct)
    {
        Debug.Log($"OnPostprocessBuild-{pathToBuiltProjecct}");
       if (target != BuildTarget.iOS)
       {
           return;
       }
        Debug.Log("OnPostprocessBuild");
        PBXProject pbx = new PBXProject();
        var pbxPath = PBXProject.GetPBXProjectPath(pathToBuiltProjecct);
        pbx.ReadFromFile(pbxPath);
        var mainTargetGuild = pbx.GetUnityMainTargetGuid();
        pbx.AddCapability(mainTargetGuild, PBXCapabilityType.SignInWithApple);
        pbx.SetBuildProperty(mainTargetGuild, "CODE_SIGN_IDENTITY", Ios_Sign_Identity);//签名使用发布证书？还是测试证书
        pbx.SetBuildProperty(mainTargetGuild, "CODE_SIGN_IDENTITY[sdk=iphoneos*]", Ios_Sign_Identity);
        pbx.SetBuildProperty(mainTargetGuild, "DEVELOPMENT_TEAM", TeamId);
        var targetGuid = pbx.GetUnityFrameworkTargetGuid();
        pbx.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
        pbx.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
        //        pbx.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-all_load");
        pbx.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-fobjc-arc");
        pbx.AddBuildProperty(targetGuid, "CODE_SIGN_STYLE", "Automatic");
        //        if (!IsDebug)
        //        {
        
        SetAppPurchase(pbx);
        //        }
        pbx.RemoveFrameworkFromProject(mainTargetGuild, "AuthenticationServices.framework");
        AddFrameworkToProject(pbx, mainTargetGuild, "AuthenticationServices.framework");
        AddFrameworkToProject(pbx, targetGuid, "Security.framework");
        AddFrameworkToProject(pbx, targetGuid, "SystemConfiguration.framework");
        AddFrameworkToProject(pbx, targetGuid, "CoreTelephony.framework");
        AddFrameworkToProject(pbx, targetGuid, "AudioToolbox.framework");
        AddFrameworkToProject(pbx, targetGuid, "CoreAudio.framework");
        AddFrameworkToProject(pbx, targetGuid, "AVFoundation.framework");
        AddFrameworkToProject(pbx, targetGuid, "StoreKit.framework");
        AddFrameworkToProject(pbx, targetGuid, "AdSupport.framework");
        AddFrameworkToProject(pbx, targetGuid, "WebKit.framework");
        //        AddFrameworkToProject(pbx, targetGuid, "libstdc++.6.0.9.tbd");
        AddFrameworkToProject(pbx, targetGuid, "libz.tbd");
        AddFrameworkToProject(pbx, targetGuid, "libc++.tbd");
        AddFrameworkToProject(pbx, targetGuid, "libsqlite3.tbd");
        AddFrameworkToProject(pbx, targetGuid, "libresolv.tbd");
       // AddImageToProject(pbx, targetGuid);  IOS Splash使用的图片，先注释掉

        pbx.WriteToFile(pbxPath);
        EditInfoPlist(pathToBuiltProjecct);
    }

    private static void SetAppPurchase(PBXProject project)
    {
        var getProject = typeof(PBXProject).GetMethod("GetProjectInternal", BindingFlags.NonPublic | BindingFlags.Instance);
        var projectInternal = getProject.Invoke(project, null);
        // 得到类型
//        Type pbxObjectDataType = typeof(PBXProject).Assembly.GetType("UnityEditor.iOS.Xcode.PBX.PBXObjectData");
        var methodGetProperiesRaw = projectInternal.GetType().GetMethod("GetPropertiesRaw", BindingFlags.Instance | BindingFlags.NonPublic);
        // 类型是PBXElementDict
        var propertiesRaw = methodGetProperiesRaw.Invoke(projectInternal, null);
        var attributeElementDict = CreateChildPBXElementDict(propertiesRaw, "attributes");
        var targetAttributeElementDict = CreateChildPBXElementDict(attributeElementDict, "TargetAttributes");

        var nativeTargetsProp =
            typeof(PBXProject).GetProperty("nativeTargets", BindingFlags.Instance | BindingFlags.NonPublic);
        var nativeTargetsValue = nativeTargetsProp.GetValue(project, null);

        var methodGetGuilds = nativeTargetsValue.GetType()
            .GetMethod("GetGuids", BindingFlags.Public | BindingFlags.Instance);
        var guids = (IEnumerable<string>) methodGetGuilds.Invoke(nativeTargetsValue, null);
        foreach (var guid in guids)
        {
//        (!pbxElementDict2.Contains(entry.Key) ? pbxElementDict2.CreateDict(entry.Key) : pbxElementDict2[entry.Key].AsDict()).SetString(key, value);
            var guidElementDict = CreateChildPBXElementDict(targetAttributeElementDict, guid);
            var systemCapabilities = CreateChildPBXElementDict(guidElementDict, "SystemCapabilities");
            var appPurchase = CreateChildPBXElementDict(systemCapabilities, "com.apple.InAppPurchase");
            SetStringInPBXElementDict(appPurchase, "enabled", "1");
        }
    }

    // 移动资源文件到新目录, 用于splashimage
    private static void AddImageToProject(PBXProject pbx, string targetGuid)
    {
        string fileName = "GameLogo.png";
        string srcPath = Path.Combine("Assets/Plugins/iOS/.sdk", fileName);
        string dstPath = Path.Combine("xcodeproj", fileName);
        File.Copy(srcPath, dstPath, true);
        string fileRef = pbx.AddFile(fileName, fileName);
        pbx.AddFileToBuild(targetGuid, fileRef);
    }

    // 由现在的PBXElementDict生成子PBXElementDict, 如果存在, 返回原有的
    private static object CreateChildPBXElementDict(object pbxElementDict, string key)
    {
         var methodContains = pbxElementDict.GetType().GetMethod("Contains");
         var methodCreateDict = pbxElementDict.GetType().GetMethod("CreateDict");
         var methodItem = pbxElementDict.GetType().GetProperty("Item");
         if (!(bool)methodContains.Invoke(pbxElementDict, new object[] {key}))
         {
             return methodCreateDict.Invoke(pbxElementDict, new object[] {key});
         }
         return methodItem.GetValue(pbxElementDict, new object[] {key});
    }

    private static void SetStringInPBXElementDict(object pbxElementDict, string key, string value)
    {
        var methodSetString = pbxElementDict.GetType().GetMethod("SetString", BindingFlags.Instance | BindingFlags.Public);
        methodSetString.Invoke(pbxElementDict, new object[]{key, value});
    }

    private static void EditInfoPlist(string path)
    {
        string plistPath = path + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        // Get root
        PlistElementDict rootDict = plist.root;
        // Change value of CFBundleDevelopmentRegion in Xcode plist
        // rootDict.SetInteger("cn.jj.sdk.packageid", 620034);
        // rootDict.SetInteger("cn.jj.sdk.promoteid", 4504750);
        // rootDict.SetInteger("cn.jj.sdk.siteid", 4504750);
        rootDict.SetString("CFBundleDevelopmentRegion", "zh_CN");
        rootDict.SetString("NSLocationWhenInUseUsageDescription", "我们需要通过您的地理位置信息获取您周边的相关数据");
        rootDict.SetString("NSMicrophoneUsageDescription", "是否允许此App使用你的麦克风");

        PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");
        // add weixin url scheme
        PlistElementDict wxUrl = urlTypes.AddDict();
        wxUrl.SetString("CFBundleTypeRole", "Editor");
        wxUrl.SetString("CFBundleURLName", "weixin");
        PlistElementArray wxUrlScheme = wxUrl.CreateArray("CFBundleURLSchemes");
        wxUrlScheme.AddString("wxfea4a1c1d7b1377e");
        // add qq url scheme
        PlistElementDict qqUrl = urlTypes.AddDict();
        qqUrl.SetString("CFBundleTypeRole", "Editor");
       // wxUrl.SetString("CFBundleURLName", "weixin");
        PlistElementArray qqUrlScheme = qqUrl.CreateArray("CFBundleURLSchemes");
        qqUrlScheme.AddString("tencent101534824");

        PlistElementArray lSApplicationQueriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
        lSApplicationQueriesSchemes.AddString("weixin");
        lSApplicationQueriesSchemes.AddString("wechat");
        lSApplicationQueriesSchemes.AddString("mqqopensdkapiv2");
        lSApplicationQueriesSchemes.AddString("mqq");
        
        rootDict.SetString("method", IosPp);
        // rootDict.SetString("UILaunchStoryboardName", "LaunchScreen");
        PlistElementDict profiles = rootDict.CreateDict("provisioningProfiles");
        profiles.SetString(PackageName, IosManualProvisioningProfileId);
        
        const string uiApplicationExitsOnSuspendKey = "UIApplicationExitsOnSuspend";
        if (rootDict[uiApplicationExitsOnSuspendKey] != null)
        {
            rootDict.values.Remove(uiApplicationExitsOnSuspendKey);
        }
        
        // Write to file
        File.WriteAllText(plistPath, plist.WriteToString());
    }

    public static void AddFrameworkToProject(PBXProject pbx, string targetGuid, string framework)
    {
        if (!pbx.ContainsFramework(targetGuid, framework))
        {
            pbx.AddFrameworkToProject(targetGuid, framework, true);
        }
    }
#endif
    //    [MenuItem("BuildTool/Jenkins/TEST_ResetFbxNormalAndTagents")]
    //    public static void TestFbxSetting()
    //    {
    //		ResetFbxNormalAndTagents("Resources_MS/Scene");
    //		ResetFbxNormalAndTagents("OldRes/Resources_MS/Scene");
    //    }


    //    public static void ResetFbxNormalAndTagents(string path)
    //    {                
    //        var dirInfo = new DirectoryInfo(Path.Combine(Application.dataPath, path));
    ////        Debug.Log(dirInfo.FullName);
    //        if (!dirInfo.Exists) return;
    //        foreach (var fileInfo in dirInfo.GetFiles())
    //        {
    //            if (fileInfo.Name.EndsWith("FBX.meta", true, null))
    //            {
    //                var yaml = new YamlStream();
    //                using (var fs = fileInfo.OpenRead())
    //                {
    //                    var sr = new StreamReader(fs);
    //                    var ss = new StringReader(sr.ReadToEnd());
    //                    yaml.Load(ss);                    
    //                    sr.Close();
    //                }
    //                var rootNode = yaml.Documents[0].RootNode;
    //                var nNode = rootNode["ModelImporter"]["tangentSpace"]["normalImportMode"] as YamlScalarNode;
    //                nNode.Value = "2";
    //                var tNode = rootNode["ModelImporter"]["tangentSpace"]["tangentImportMode"] as YamlScalarNode;
    //                tNode.Value = "2";
    //				var rNode = rootNode["ModelImporter"]["animations"]["isReadable"] as YamlScalarNode;
    //				rNode.Value = "0";
    //                using (var fss = fileInfo.OpenWrite())
    //                {
    //                    var sw = new StreamWriter(fss);
    //                    yaml.Save(sw,false);
    //                    sw.Flush();
    //                }
    //#if UNITY_EDITOR
    //                Debug.LogFormat("reset file: {0}", fileInfo.Name);
    //#else
    //                Console.WriteLine("reset file: {0}", fileInfo.Name);
    //#endif
    //            }
    //        }

    //        foreach (var di in dirInfo.GetDirectories())
    //        {
    //            ResetFbxNormalAndTagents(di.FullName);
    //        }
    //    }
    public static void SaveFile(string path,string text)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.Write(text);
        }
    }
}

//public class MyBuildPostprocessor
//{
//    [PostProcessBuild(1)]
//    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
//    {
//        File.Delete("Library/ScriptAssemblies/Assembly-CSharp.dll");
//        Debug.Log(pathToBuiltProject);
//    }

//    [PostProcessSceneAttribute(1)]
//    public static void OnPostprocessScene()
//    {
//        File.Delete("Library/ScriptAssemblies/Assembly-CSharp.dll");
//        //Debug.Log(pathToBuiltProject);
//    }
//}
public class BuildBatClass
{
    
    public static void StartCmd(string[] Command)
    {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
#if UNITY_EDITOR_OSX
        p.StartInfo.FileName = @"/bin/bash";//设定需要执行的命令
#elif UNITY_EDITOR_WIN
        p.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";//设定需要执行的命令
#endif
        p.StartInfo.UseShellExecute = true; //是否执行shell
        p.StartInfo.RedirectStandardInput = false;
        p.StartInfo.RedirectStandardOutput = false;
        p.StartInfo.Arguments = "";// 单个命令
        for (int i = 0; i < Command.Length; i++)
        {
            p.StartInfo.Arguments += ("/k " + Command[i]);
        }
        p.Start();

        //p.StandardInput.WriteLine(Command);
        // output = p.StandardOutput.ReadToEnd();
        //p.StandardInput.AutoFlush = true;
        //p.StandardInput.Close();
        p.WaitForExit();//等待程序执行完退出进程
        p.Close();
    }
    public static void RevertGitPath(string path)
    {
        string[] commond = new string[] { $@"git reset HEAD {path}", "git checkout ${path}" };
        StartCmd(commond);
    }
    
}
