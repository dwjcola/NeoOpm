//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.AddressableResource;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using NeoOPM;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace NeoOPM
{

    public class ProcedureCheckVersion : ProcedureBase
    {
        private bool m_InitResourcesComplete = false;
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
        //存放的是从 Addressable.ResourceLocators 检查到存在更新的所有的key，被DownloadDependenciesAsync使用
        List<IEnumerable<object>> UpdateLocatorKeys = new List<IEnumerable<object>>();
        CheckUpdateForm checkUpdateForm;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_InitResourcesComplete = false;
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnUIOpenSuccess);
            GameEntry.UI.StartCoroutine(OnInit());
        }
        public IEnumerator OnInit()
        {
            PlatformConfig.InitConfig();//修改更新地址
            yield return Addressables.InitializeAsync();//初始化
            GameEntry.Scene.UnloadAllScene();

            Log.Info("lmm-----OnEnterProcedureCheckVersion");
            AASResourceManager.Instance.UnLoadAllAssetCache();

            Log.Info("lmm-----StartLoadUICheckUpdate");
            GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset(CheckUpdateForm.UIAssetName), "Default", this);//这个UI不和其他UI放到一个Group
        }
        /// <summary>
        /// 初始化之后，若网络不行，可能catalog也更新失败了，先检查网络状态，再检查catalog，更新catalog之后再检查更新文件内容
        /// 在网络状态良好的时候会存在多检查一次catalog的情况，但是没有关系
        /// 这里就可以考虑勾选 ：Disable Catalog Update On Startup 了。这样初始化的时候没有检查catalog，这里再检查就不算是无用操作了
        /// </summary>
        public void CheckUpdateImpl()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                //无网络
                checkUpdateForm.ShowErrorMessage("", "网络连接失败，请检查网络重试", () =>
                {
                    GameEntry.UI.StartCoroutine(WaitCheckInternet());
                });// 点击重试
            }
            else
            {
                //确保调用InitializeAsync的时候网络状态是良好的，不然catalog和hash更不下来，只会更新apk里当前catalog里的RemoteLoad文件，不能检测到新的更新
                CheckIsUpdate();
            }
        }
        public IEnumerator WaitCheckInternet()
        {
            yield return new WaitForSeconds(0.2f);
            CheckUpdateImpl();
        }
        public void CheckIsUpdate()
        {
            if (IsAAsUseingExitingBuild)
            {
                GameEntry.UI.StartCoroutine(CheckCatalog());
            }
            else
            {
                checkUpdateForm.preloadState = PreloadState.NoUpdate;
                AfterUpdateDone();
            }
        }
        public static bool IsAAsUseingExitingBuild
        {
            get
            {
#if UNITY_EDITOR
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
                return settings.ActivePlayModeDataBuilderIndex == index;
#else
                return true;
#endif
            }
        }
        private void OnUIOpenSuccess(object sender, GameEventArgs e)
        {

            OpenUIFormSuccessEventArgs evn = (OpenUIFormSuccessEventArgs)e;
            if (evn.UIForm.Logic is CheckUpdateForm)
            {
                checkUpdateForm = (CheckUpdateForm)evn.UIForm.Logic;
            }
            Log.Info("OpenUI-"+evn.UIForm.Logic.Name);

            checkUpdateForm.preloadState = PreloadState.Init;
            if (Application.isEditor)
            {
                AfterUpdateDone();
                return;
            }
            CheckUpdateImpl();

        }

        IEnumerator CheckCatalog()
        {
            Log.Info("lmm-----StartCheckCataLog TextDataProvider");
            AsyncOperationHandle<List<string>> catalogHandle = Addressables.CheckForCatalogUpdates(false);
            yield return catalogHandle;
            Log.Info($"lmm-----CheckCatalog status={catalogHandle.Status}");
            if (catalogHandle.Status == AsyncOperationStatus.Succeeded)
            {
                if (catalogHandle.Result?.Count > 0)//catalog有更新的情况下先更新catalog
                {
                    var updateCatalogsHandle = Addressables.UpdateCatalogs(catalogHandle.Result, false);//自动释放就行，从Addressables.ResourceLocator 获取要更新的keys
                    yield return updateCatalogsHandle;

                    Addressables.Release(updateCatalogsHandle);
                }
                GameEntry.UI.StartCoroutine(CheckUpdate());
            }
            else
            {
                GameEntry.UI.StopCoroutine(CheckCatalog());
                Log.Warning($"CheckCatalog statues=fail-{catalogHandle.OperationException.Message}");
                checkUpdateForm.ShowErrorMessage("", "检查更新文件失败，请检查网络后重试", () =>
                {
                    GameEntry.UI.StartCoroutine(CheckCatalog());
                });
            }
            Addressables.Release(catalogHandle);
        }
        private IEnumerator CheckUpdate()
        {
            //先按照不勾选，自动更新catalog来
            //只要打包的时候不要将Disable  Catalog Update on Startup勾选上就行，这样初始化的时候会自动更新Catalog到最新
            long UpdateByteSize = 0;
            UpdateLocatorKeys.Clear();
            IEnumerable<IResourceLocator> locators = Addressables.ResourceLocators;
            IEnumerator<IResourceLocator> locator = locators.GetEnumerator();
            while (locator.MoveNext())
            {
                AsyncOperationHandle<long> locatorHandle = Addressables.GetDownloadSizeAsync(locator.Current.Keys);
                yield return locatorHandle;
                if (locatorHandle.IsDone && locatorHandle.Status == AsyncOperationStatus.Succeeded && locatorHandle.Result > 0)
                {
                    UpdateByteSize += locatorHandle.Result;
                    UpdateLocatorKeys.Add(locator.Current.Keys);
                }
                Addressables.Release(locatorHandle);
            }
            checkUpdateForm.preloadState = PreloadState.CheckDone;
            Log.Debug($"<color=green>addressable 检查更新，大小={UpdateByteSize}</color>");
            checkUpdateForm.LoadingDone = true;
            if (UpdateByteSize > 0)
            {
                //检查到更新 文件 

                //TODO 更新提醒，确认更新？通知UI
                checkUpdateForm?.ShowUpdateMessage("更新", $"检查到更新内容，文件大小为{FileSize(UpdateByteSize)}", ConfirmUpdate);
            }
            else
            {
                //无更新 TODO
                Log.Debug("<color=green>检查到没有更新内容</color>");
                checkUpdateForm.preloadState = PreloadState.NoUpdate;
                AfterUpdateDone();
            }
        }
        public void ConfirmUpdate()
        {
            GameEntry.UI.StartCoroutine(DownloadDependencyImpl());
        }
        IEnumerator DownloadDependencyImpl()
        {
            checkUpdateForm.ChangeLoadState();
            checkUpdateForm.preloadState = PreloadState.HotUpdate;
            List<AsyncOperationHandle> dowloadHandles = new List<AsyncOperationHandle>();
            for (int i = 0; i < UpdateLocatorKeys.Count; i++)
            {
                dowloadHandles.Add(Addressables.DownloadDependenciesAsync(UpdateLocatorKeys[i], Addressables.MergeMode.Union));
            }
            float percentage = 0;
            while (percentage < dowloadHandles.Count)
            {
                percentage = 0f;
                for (int i = 0; i < dowloadHandles.Count; i++)
                {
                    percentage += dowloadHandles[i].PercentComplete;
                    // dowloadHandles[i].GetDownloadStatus().DownloadedBytes
                }
                //检查下载进度
                string progressStr = $"下载进度:{percentage / dowloadHandles.Count * 100}%";
                //UI表现
                //进度条变化，进度修改
                checkUpdateForm.UpdateSliderValue(percentage / dowloadHandles.Count);
                yield return null;
            }
            for (int i = 0; i < dowloadHandles.Count; i++)
            {
                Addressables.Release(dowloadHandles[i]);
            }
            dowloadHandles.Clear();
            string downLoadOver = "更新完成";

            //checkUpdateForm.preloadState = PreloadState.HotUpdateDone;
            //  切换到下一流程 切换场景？或者预加载某些资源？
            AfterUpdateDone();
        }

        private string FileSize(long size)
        {
            string ret = string.Empty;
            float mbyte = size / 1000000f;
            if (mbyte > 1)
            {
                ret = string.Format("{0:F1}M", mbyte);
            }
            else
            {
                float kbyte = size / 1000f;
                if (kbyte > 1)
                {
                    ret = string.Format("{0:F1}K", kbyte);
                }
                else
                {
                    ret = "1K";
                }
            }

            return ret;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_InitResourcesComplete)
            {
                return;
            }

            ChangeState<ProcedurePreload>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnUIOpenSuccess);
            //if (checkUpdateForm!=null)
            //{
            //    GameEntry.UI.CloseUIForm(checkUpdateForm);
            //}

            checkUpdateForm = null;
            base.OnLeave(procedureOwner, isShutdown);
        }
        List<AsyncOperationHandle> PreloadAssets = new List<AsyncOperationHandle>();
        /// <summary>
        ///更新完成之后
        /// </summary>
        public void AfterUpdateDone()
        {
            UnityGameFramework.Runtime.GameEntry.GetComponent<AddressableResourceComponent>()?.LoadAssetsAfterUpdate();
            m_InitResourcesComplete = true;
            checkUpdateForm.LoadingDone = true;
        }
    }
}
