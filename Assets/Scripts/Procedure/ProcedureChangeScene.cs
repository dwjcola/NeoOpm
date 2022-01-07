//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine.AddressableAssets;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.AddressableResource;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using XLua;

namespace NeoOPM
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private bool m_IsChangeSceneComplete = false;
        private ProcedureOwner _procedureOwner;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _procedureOwner = procedureOwner;

            m_IsChangeSceneComplete = false;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);


            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            if (loadedSceneAssetNames.Length > 0)
            {
                GameEntry.Event.Subscribe(UnloadSceneSuccessEventArgs.EventId, OnUnloadSceneSuccess);
                for (int i = 0; i < loadedSceneAssetNames.Length; i++)
                {
                    GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
                }
            }
            else 
            {
                LoadScene(procedureOwner);
            }
            
            // 还原游戏速度
        }
        float progress = 0;
        float _Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                GameEntry.Event.Fire(this, LUAEventArgs.Create(UpdateLoadingPrgress.GetHashCode(), progress));
            }
        }
        private void LoadScene(ProcedureOwner procedureOwner)
        {
            GameEntry.Base.ResetNormalGameSpeed();
            LC.SendEvent("SHOW_LOADING_UI", true);
            int sceneId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
            PreLoadAssets(sceneId);
        }

        public void PreLoadAssets(int sceneId)
        {
            /*AASResourceManager.Instance.ReleaseCache(CacheUnloadTime.ChangeScene);
            var preLoadAssetList = AASResourceManager.Instance.PreLoadAssetsLoadScene(sceneId);
            int totalCount = preLoadAssetList.Count;
            int loadCount = 0;
            _Progress = 0;
            AASResourceManager.Instance.CacheResourceAsync(preLoadAssetList, (obj) => {
                ++loadCount;
                _Progress = 0.9f * loadCount / totalCount;
            }, () => {*/
            LuaTable drScene = LC.GetTable("Scene", sceneId);
            
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
                return;
            }
            string AssetName = drScene.Get<String>("AssetName");
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(AssetName), Constant.AssetPriority.SceneAsset, this);
            /*});*/
        }
        private void OnUnloadSceneSuccess(object sender, GameEventArgs e)
        {
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            if (loadedSceneAssetNames.Length == 0)
            {
                GameEntry.Event.Unsubscribe(UnloadSceneSuccessEventArgs.EventId, OnUnloadSceneSuccess);
                LoadScene(_procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            LC.SendEvent("SHOW_LOADING_UI", false);
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            //m_IsChangeSceneComplete LoadScene isDone?
            //isLoadDone PreLoadAssets isDone?
            if (!m_IsChangeSceneComplete)
            {
                return;
            }
            int sceneId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
            if (sceneId == Constant.Scene.City)
            {
                ChangeState<ProcedureCity>(procedureOwner);
            }
            else if (sceneId == Constant.Scene.Battle)
            {
                ChangeState<ProcedureBattle>(procedureOwner);
            }
            else
            {
                //ChangeState<ProcedureWorld>(procedureOwner);
            }
        }

        private async void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            /*if (m_BackgroundMusicId > 0)
            {
                GameEntry.WWise.PlayBGM(m_BackgroundMusicId);
            }*/
            _Progress = 1;
           // await InvokeAllTask();
            m_IsChangeSceneComplete = true;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }
        const string UpdateLoadingPrgress="UPDATE_LOADING_PROGRESS";
        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            _Progress = _Progress+ ne.Progress * (1 - _Progress);
            //GameEntry.Event.Fire(sender, LUAEventArgs.Create(UpdateLoadingPrgress.GetHashCode(), ne.Progress));
            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }
    }
}
