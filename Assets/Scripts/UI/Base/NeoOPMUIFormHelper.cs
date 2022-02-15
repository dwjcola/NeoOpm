﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.UI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityGameFramework.Runtime;

namespace NeoOPM
{
    /// <summary>
    /// 默认界面辅助器。
    /// </summary>
    public class NeoOPMUIFormHelper : UIFormHelperBase
    {
        //private ResourceComponent m_ResourceComponent = null;
        private AddressableResourceComponent m_ResourceComponent = null;

        /// <summary>
        /// 实例化界面。
        /// </summary>
        /// <param name="uiFormAsset">要实例化的界面资源。</param>
        /// <returns>实例化后的界面。</returns>
        public override object InstantiateUIForm(object uiFormAsset)
        {
            return m_ResourceComponent.InstantiateAsset(uiFormAsset);
        }

        /// <summary>
        /// 创建界面。
        /// </summary>
        /// <param name="uiFormInstance">界面实例。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面。</returns>
        public override IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
        {
            GameObject gameObject = uiFormInstance as GameObject;
            if (gameObject == null)
            {
                Log.Error("UI form instance is invalid.");
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent(((MonoBehaviour)uiGroup.Helper).transform,false);
            transform.localScale = Vector3.one;

            return gameObject.GetOrAddComponent<UIForm>();
        }

        /// <summary>
        /// 释放界面。
        /// </summary>
        /// <param name="uiFormAsset">要释放的界面资源。</param>
        /// <param name="uiFormInstance">要释放的界面实例。</param>
        public override void ReleaseUIForm(object uiFormAsset, object uiFormInstance)
        {
            GameObject go = (GameObject) uiFormInstance;
            LC.RealeasAtlas(go);
            var resMgr = GameFramework.GameFrameworkEntry.GetModule<GameFramework.AddressableResource.IAddressableResourceManager>();
            resMgr.ReleaseInstantiateAsset(go);
            resMgr.UnloadAsset(uiFormAsset);
            //m_ResourceComponent.UnloadAsset(uiFormAsset);
            //Destroy((Object)uiFormInstance);
        }

        private void Start()
        {
            m_ResourceComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<AddressableResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Addressable Resource Component is invalid.");
                return;
            }
        }
    }
}
