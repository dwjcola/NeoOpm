using GameFramework;
using System;
using UnityEngine;
using GameFramework.AddressableResource;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 资源组件。
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class AddressableResourceComponent : GameFrameworkComponent
    {
        private IAddressableResourceManager m_AddressableResourceManager = null;
        public bool IsUpdated
        {
            get;
            set;
        }
        Action LoadAssetAction;
        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start() 
        {
            m_AddressableResourceManager = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
            if (m_AddressableResourceManager == null)
            {
                Log.Fatal("Addressable ResourceManager is invalid.");
                return;
            }
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetKey">资源路径名</param>
        /// <returns></returns>
        public object InstantiateAsset(object assetKey)
        {
            return m_AddressableResourceManager.InstantiateAsset(assetKey); // Instantiate((Object)uiFormAsset);
        }


        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            m_AddressableResourceManager.UnloadAsset(asset);
        }
        /// <summary>
        /// 注册一个在检查更新完成之后加载资源的接口
        /// </summary>
        public void RegisterLoadAssetsStep(Action action)
        {
            if (IsUpdated)
            {
                action?.Invoke();
                return;
            }
            LoadAssetAction += action;
        }
        public void LoadAssetsAfterUpdate()
        {
            LoadAssetAction?.Invoke();
            LoadAssetAction = null;
        }
        private void OnDestroy()
        {
            LoadAssetAction=null;
        }

        public void ForceUnloadUnusedAssets(bool performGCCollect)
        {
            //TODO
        }
    }

    


}
