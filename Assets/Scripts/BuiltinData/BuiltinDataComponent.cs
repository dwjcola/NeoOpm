//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityGameFramework.Runtime;

namespace ProHA
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField]
        private TextAsset m_BuildInfoTextAsset = null;

        //[SerializeField]
        //private AssetReference m_BuildInfoAsset;

        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;

        //[SerializeField]
        //private AssetReference m_DefaultDictionaryAsset;

        private BuildInfo m_BuildInfo = null;

        public BuildInfo BuildInfo
        {
            get
            {
                return m_BuildInfo;
            }
        }
        public void Start()
        {
            InitBuildInfo();
            InitDefaultDictionary();
        }
        public void InitBuildInfo()
        {
            #region  addressableLoad
            //if (m_BuildInfoAsset == null||!m_BuildInfoAsset.IsValid())
            //{
            //    return;
            //}
            //var h= m_BuildInfoAsset.LoadAssetAsync<TextAsset>();
            //h.WaitForCompletion();
            //if (!h.IsValid() || h.Result == null || h.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
            //{
            //    return;
            //}
            //m_BuildInfo = Utility.Json.ToObject<BuildInfo>(h.Result.text);
            #endregion
            if (m_BuildInfoTextAsset == null || string.IsNullOrEmpty(m_BuildInfoTextAsset.text))
            {
                Log.Info("Build info can not be found or empty.");
                return;
            }

            m_BuildInfo = Utility.Json.ToObject<BuildInfo>(m_BuildInfoTextAsset.text);
            if (m_BuildInfo == null)
            {
                Log.Warning("Parse build info failure.");
                return;
            }
        }

        public void InitDefaultDictionary()
        {
            #region addressableLoad
            //if (m_DefaultDictionaryAsset == null || !m_DefaultDictionaryAsset.IsValid())
            //{
            //    return;
            //}
            //var h = m_DefaultDictionaryAsset.LoadAssetAsync<TextAsset>();
            //h.WaitForCompletion();
            //if (!h.IsValid() || h.Result == null || h.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
            //{
            //    return;
            //}
            //if (!GameEntry.Localization.ParseData(h.Result.text))
            //{
            //    Log.Warning("Parse default dictionary failure.");
            //    return;
            //}
            #endregion
            if (m_DefaultDictionaryTextAsset == null || string.IsNullOrEmpty(m_DefaultDictionaryTextAsset.text))
            {
                Log.Info("Default dictionary can not be found or empty.");
                return;
            }

            if (!GameEntry.Localization.ParseData(m_DefaultDictionaryTextAsset.text))
            {
                Log.Warning("Parse default dictionary failure.");
                return;
            }

        }
    }
}
