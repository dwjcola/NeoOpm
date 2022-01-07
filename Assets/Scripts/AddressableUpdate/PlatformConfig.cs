using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace NeoOPM
{
    public class PlatformConfig
    {
        public static readonly string PersistentDirName = @"KOSPersist";
#if UNITY_EDITOR
        // 自定义持久化目录
        public static readonly string AppPersistentDataPath = Combine(Directory.GetCurrentDirectory(), "Persist").Replace(@"\", @"/");
#else
        public static readonly string AppPersistentDataPath = Application.persistentDataPath.Replace(@"\", @"/");
#endif
        public static readonly string PersistentDataPath = Combine(AppPersistentDataPath, PersistentDirName);
        public static readonly string StreamingAssetPath = Application.streamingAssetsPath.Replace(@"\", @"/");
        public static readonly string LibraryPath = Path.Combine(Application.dataPath, "Library");
        public static readonly string PlatformConfigFile = @"platform.json";
        static string m_RemoteBundleURL = "";
        static bool init = false;
        public static string Combine(string path1, string path2)
        {
            return path1 + "/" + path2;
        }
        public static string RemoteBundleURL
        {
            get
            {
                if (!init)
                    InitConfig();
                return m_RemoteBundleURL;
            }
        }
        static void ParseAll(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return;
            var data = JsonMapper.ToObject(txt);
            foreach (var VARIABLE in data.Keys)
            {
                if (VARIABLE == kRemoteLoadPath)
                {
                    m_RemoteBundleURL = data[VARIABLE].ToString();
                }
            }
        }
        public const string kRemoteLoadPath = "RemoteLoadPath";
        public const string RemoteConfigPath = "RemoteConfig.json";
        public static async void InitConfig()
        {
            var platformFile = Path.Combine(PersistentDataPath, PlatformConfigFile);
            if (File.Exists(platformFile))
            {
                string platformFileText = File.ReadAllText(platformFile);
                ParseAll(platformFileText);
            }
            else
            {
                //在addressable初始化的时候会取到catalog中的所有location,修正其中的地址
                //这里不能使用addressable加载，否则初始化ResourceLocates会导致remotePath不对
                //故采用在打包时写入streamingassets一个文件，包含当前使用的内容
                System.Uri uri = new System.Uri(Path.Combine(Application.streamingAssetsPath, RemoteConfigPath));
                UnityWebRequest request = UnityWebRequest.Get(uri);
                TaskCompletionSource<string> task = new System.Threading.Tasks.TaskCompletionSource<string>();
                request.SendWebRequest().completed += (h) => { task.SetResult(request.downloadHandler.text); };
                string RemoteConfig = await task.Task;
                ParseAll(RemoteConfig);
            }
            init = true;
        }

        public static bool OpenGM
        {
            get
            {
#if OPENGM || UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }
    }
}
