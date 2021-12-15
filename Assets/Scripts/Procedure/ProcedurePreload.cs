//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.AddressableResource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ProHA
{
    public class ProcedurePreload : ProcedureBase
    {

        List<AsyncOperationHandle> PreloadAssets = new List<AsyncOperationHandle>();
        bool PreLoadDone = false;
        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            PreLoadAssetsAsync();
            
            //WorldTileManager.Instance.InitData();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (PreLoadDone == false|| !CheckUpdateForm.Instance.IsProgressDone())
                return;
            
             ChangeState<ProcedureLogin>(procedureOwner);
            
        }
        public void PreLoadAssetsAsync()
        {
            CheckUpdateForm.Instance.ChangeLoadState();
            CheckUpdateForm.Instance.preloadState = PreloadState.PreLoading;
            AASResourceManager.Instance.InstantiateAsset("Assets/Resource_MS/Prefabs/Customs/Customs.prefab",
                new UnityEngine.ResourceManagement.ResourceProviders.InstantiationParameters(GameEntry.Base.transform.parent,false));
            ///担心在Complete里写能不能保证执行;
            var loadTableHandle = Addressables.LoadAssetsAsync<TextAsset>("Tables", (text) =>
            {
                TableTools.instance.PreLoadTable(text);
            });
            PreloadAssets.Add(loadTableHandle);

            var loadLuaHandle = Addressables.LoadAssetsAsync<TextAsset>("LuaScripts", (text) =>
            {
                if (text != null)
                {
                    XluaManager.instance.InitLuaMap(text);
                }
            });
            PreloadAssets.Add(loadLuaHandle);
            
            PreloadAssets.Add( LoadFont("MainFont"));
            //GameEntry.numTextProComponent.Initialized();
            GameEntry.UI.StartCoroutine(PreLoadCoroutine());
        }
        private  AsyncOperationHandle LoadFont(string fontName)
        {
            var resMgr = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
            var loadFontHandle= AASResourceManager.Instance.LoadAssetHandleAsync<Font>(AssetUtility.GetFontAsset(fontName), (name,isSuccess,font) =>
            {
                UGuiForm.SetMainFont(font);
                Log.Info("Load font '{0}' OK.", fontName);
            });
            return loadFontHandle;
        }
        IEnumerator PreLoadCoroutine()
        {
            while (PreloadAssets.Count > 0)
            {
                int totalPreLoad = PreloadAssets.Count;
                for (int i = totalPreLoad - 1; i > -1; i--)
                {
                    if (PreloadAssets[i].IsDone)
                    {
                        Addressables.Release(PreloadAssets[i]);
                        PreloadAssets.RemoveAt(i);
                    }
                }
                yield return null;
            }
            PreInit();
            PreLoadDone = true;
            CheckUpdateForm.Instance.LoadingDone = true;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void PreInit()
        {
            XluaManager.instance.Init();
            ServerTime.Init();
            GameEntry.UI.OpenUI("LoadingUIForm", -1);
            UIReminder.InitReminder();
        }
    }
}
