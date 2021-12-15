//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using GameFramework.AddressableResource;
using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameFramework.Scene
{
    /// <summary>
    /// 场景管理器。
    /// </summary>
    internal sealed class SceneManager : GameFrameworkModule, ISceneManager
    {
        private readonly Dictionary<string,SceneInstance> m_LoadedSceneAssetNamesDic;
        private readonly List<string> m_LoadingSceneAssetNames;
        private readonly List<string> m_UnloadingSceneAssetNames;
        private readonly LoadSceneCallbacks m_LoadSceneCallbacks;
        private readonly UnloadSceneCallbacks m_UnloadSceneCallbacks;
        private IAddressableResourceManager m_ResourceManager;
        private EventHandler<LoadSceneSuccessEventArgs> m_LoadSceneSuccessEventHandler;
        private EventHandler<LoadSceneFailureEventArgs> m_LoadSceneFailureEventHandler;
        private EventHandler<LoadSceneUpdateEventArgs> m_LoadSceneUpdateEventHandler;
        private EventHandler<LoadSceneDependencyAssetEventArgs> m_LoadSceneDependencyAssetEventHandler;
        private EventHandler<UnloadSceneSuccessEventArgs> m_UnloadSceneSuccessEventHandler;
        private EventHandler<UnloadSceneFailureEventArgs> m_UnloadSceneFailureEventHandler;

        private LoadSceneUpdateArgs loadSceneUpdate;

        /// <summary>
        /// 初始化场景管理器的新实例。
        /// </summary>
        public SceneManager()
        {
            m_LoadedSceneAssetNamesDic = new Dictionary<string, SceneInstance>();
            m_LoadingSceneAssetNames = new List<string>();
            m_UnloadingSceneAssetNames = new List<string>();
            m_LoadSceneCallbacks = new LoadSceneCallbacks(LoadSceneSuccessCallback, LoadSceneFailureCallback, LoadSceneUpdateCallback, LoadSceneDependencyAssetCallback);
            m_UnloadSceneCallbacks = new UnloadSceneCallbacks(UnloadSceneSuccessCallback, UnloadSceneFailureCallback);
            m_ResourceManager = null;
            m_LoadSceneSuccessEventHandler = null;
            m_LoadSceneFailureEventHandler = null;
            m_LoadSceneUpdateEventHandler = null;
            m_LoadSceneDependencyAssetEventHandler = null;
            m_UnloadSceneSuccessEventHandler = null;
            m_UnloadSceneFailureEventHandler = null;
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return 60;
            }
        }

        /// <summary>
        /// 加载场景成功事件。
        /// </summary>
        public event EventHandler<LoadSceneSuccessEventArgs> LoadSceneSuccess
        {
            add
            {
                m_LoadSceneSuccessEventHandler += value;
            }
            remove
            {
                m_LoadSceneSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载场景失败事件。
        /// </summary>
        public event EventHandler<LoadSceneFailureEventArgs> LoadSceneFailure
        {
            add
            {
                m_LoadSceneFailureEventHandler += value;
            }
            remove
            {
                m_LoadSceneFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载场景更新事件。
        /// </summary>
        public event EventHandler<LoadSceneUpdateEventArgs> LoadSceneUpdate
        {
            add
            {
                m_LoadSceneUpdateEventHandler += value;
            }
            remove
            {
                m_LoadSceneUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载场景时加载依赖资源事件。
        /// </summary>
        public event EventHandler<LoadSceneDependencyAssetEventArgs> LoadSceneDependencyAsset
        {
            add
            {
                m_LoadSceneDependencyAssetEventHandler += value;
            }
            remove
            {
                m_LoadSceneDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 卸载场景成功事件。
        /// </summary>
        public event EventHandler<UnloadSceneSuccessEventArgs> UnloadSceneSuccess
        {
            add
            {
                m_UnloadSceneSuccessEventHandler += value;
            }
            remove
            {
                m_UnloadSceneSuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 卸载场景失败事件。
        /// </summary>
        public event EventHandler<UnloadSceneFailureEventArgs> UnloadSceneFailure
        {
            add
            {
                m_UnloadSceneFailureEventHandler += value;
            }
            remove
            {
                m_UnloadSceneFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 场景管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if(loadSceneUpdate!=null)
            {
                //UnityEngine.Debug.LogError(loadSceneUpdate.handle.IsDone+"------");
                if ((loadSceneUpdate.handle.IsValid() && !loadSceneUpdate.handle.IsDone))
                {
                    float progress = loadSceneUpdate.handle.PercentComplete;
                    LoadSceneUpdateCallback(loadSceneUpdate.args.SceneAssetName, progress, loadSceneUpdate.args.UserData);
                }
                else
                {
                    loadSceneUpdate.Release();
                    loadSceneUpdate = null;
                }
               
            }
        }

        /// <summary>
        /// 关闭并清理场景管理器。
        /// </summary>
        internal override void Shutdown()
        {
            foreach (string loadedSceneAssetName in m_LoadedSceneAssetNamesDic.Keys)
            {
                if (SceneIsUnloading(loadedSceneAssetName))
                {
                    continue;
                }
                
                UnloadScene(loadedSceneAssetName);
            }

            m_LoadedSceneAssetNamesDic.Clear();
            m_LoadingSceneAssetNames.Clear();
            m_UnloadingSceneAssetNames.Clear();
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IAddressableResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 获取场景是否已加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否已加载。</returns>
        public bool SceneIsLoaded(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            return m_LoadedSceneAssetNamesDic.ContainsKey(sceneAssetName);
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <returns>已加载场景的资源名称。</returns>
        public string[] GetLoadedSceneAssetNames()
        {
            string[] keys = new string[m_LoadedSceneAssetNamesDic.Keys.Count];
            m_LoadedSceneAssetNamesDic.Keys.CopyTo(keys, 0);
            return keys; 
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <param name="results">已加载场景的资源名称。</param>
        public void GetLoadedSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_LoadedSceneAssetNamesDic.Keys);
        }

        /// <summary>
        /// 获取场景是否正在加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在加载。</returns>
        public bool SceneIsLoading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            return m_LoadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <returns>正在加载场景的资源名称。</returns>
        public string[] GetLoadingSceneAssetNames()
        {
            return m_LoadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <param name="results">正在加载场景的资源名称。</param>
        public void GetLoadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_LoadingSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在卸载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在卸载。</returns>
        public bool SceneIsUnloading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            return m_UnloadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <returns>正在卸载场景的资源名称。</returns>
        public string[] GetUnloadingSceneAssetNames()
        {
            return m_UnloadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <param name="results">正在卸载场景的资源名称。</param>
        public void GetUnloadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_UnloadingSceneAssetNames);
        }

        ///// <summary>
        ///// 检查场景资源是否存在。
        ///// </summary>
        ///// <param name="sceneAssetName">要检查场景资源的名称。</param>
        ///// <returns>场景资源是否存在。</returns>
        //public bool HasScene(string sceneAssetName)
        //{
        //    return m_ResourceManager.HasAsset(sceneAssetName) != HasAssetResult.NotExist;
        //}

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        public void LoadScene(string sceneAssetName)
        {
            LoadScene(sceneAssetName, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        public void LoadScene(string sceneAssetName, int priority)
        {
            LoadScene(sceneAssetName, priority, null);
        }

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadScene(string sceneAssetName, object userData)
        {
            LoadScene(sceneAssetName, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="priority">加载场景资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadScene(string sceneAssetName, int priority, object userData)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (SceneIsUnloading(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is being unloaded.", sceneAssetName));
            }

            if (SceneIsLoading(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is being loaded.", sceneAssetName));
            }

            if (SceneIsLoaded(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is already loaded.", sceneAssetName));
            }
            m_LoadingSceneAssetNames.Add(sceneAssetName);
            //m_ResourceManager.LoadScene(sceneAssetName, m_LoadSceneCallbacks, userData);
            var handler = m_ResourceManager.LoadSceneAsync(sceneAssetName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            loadSceneUpdate =new LoadSceneUpdateArgs(handler, sceneAssetName, userData) ;
            handler.Completed += (handle) =>
            {
                var asset = handle.Result;
                m_LoadSceneCallbacks.LoadSceneSuccessCallback?.Invoke(sceneAssetName, asset, 0, userData);
            };
            
        }
        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        public void UnloadScene(string sceneAssetName)
        {
            if (m_LoadedSceneAssetNamesDic.ContainsKey(sceneAssetName))
            {
                UnloadScene(sceneAssetName, m_LoadedSceneAssetNamesDic[sceneAssetName], null);
            }
        }

        public void UnloadScene(string sceneAssetName, object userData)
        {
            if (m_LoadedSceneAssetNamesDic.ContainsKey(sceneAssetName))
            {
                UnloadScene(sceneAssetName, m_LoadedSceneAssetNamesDic[sceneAssetName], userData);
            }
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void UnloadScene(string sceneAssetName, SceneInstance sceneInstance, object userData)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GameFrameworkException("Scene asset name is invalid.");
            }

            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (SceneIsUnloading(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is being unloaded.", sceneAssetName));
            }

            if (SceneIsLoading(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is being loaded.", sceneAssetName));
            }

            if (!SceneIsLoaded(sceneAssetName))
            {
                throw new GameFrameworkException(Utility.Text.Format("Scene asset '{0}' is not loaded yet.", sceneAssetName));
            }

            m_UnloadingSceneAssetNames.Add(sceneAssetName);
            m_ResourceManager.UnloadScene(sceneAssetName, sceneInstance, m_UnloadSceneCallbacks, userData);
        }

        private void LoadSceneSuccessCallback(string sceneAssetName, SceneInstance sceneIns, float duration, object userData)
        {
            m_LoadingSceneAssetNames.Remove(sceneAssetName);
            if (!m_LoadedSceneAssetNamesDic.ContainsKey(sceneAssetName))
            {
                m_LoadedSceneAssetNamesDic.Add(sceneAssetName, sceneIns);
            }
            if (m_LoadSceneSuccessEventHandler != null)
            {
                LoadSceneSuccessEventArgs loadSceneSuccessEventArgs = LoadSceneSuccessEventArgs.Create(sceneAssetName, duration, userData);
                m_LoadSceneSuccessEventHandler(this, loadSceneSuccessEventArgs);
                ReferencePool.Release(loadSceneSuccessEventArgs);
            }
        }

        private void LoadSceneFailureCallback(string sceneAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            m_LoadingSceneAssetNames.Remove(sceneAssetName);
            string appendErrorMessage = Utility.Text.Format("Load scene failure, scene asset name '{0}', status '{1}', error message '{2}'.", sceneAssetName, status.ToString(), errorMessage);
            if (m_LoadSceneFailureEventHandler != null)
            {
                LoadSceneFailureEventArgs loadSceneFailureEventArgs = LoadSceneFailureEventArgs.Create(sceneAssetName, appendErrorMessage, userData);
                m_LoadSceneFailureEventHandler(this, loadSceneFailureEventArgs);
                ReferencePool.Release(loadSceneFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadSceneUpdateCallback(string sceneAssetName, float progress, object userData)
        {
            if (m_LoadSceneUpdateEventHandler != null)
            {
                LoadSceneUpdateEventArgs loadSceneUpdateEventArgs = LoadSceneUpdateEventArgs.Create(sceneAssetName, progress, userData);
                m_LoadSceneUpdateEventHandler(this, loadSceneUpdateEventArgs);
                ReferencePool.Release(loadSceneUpdateEventArgs);
            }
        }

        private void LoadSceneDependencyAssetCallback(string sceneAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            if (m_LoadSceneDependencyAssetEventHandler != null)
            {
                LoadSceneDependencyAssetEventArgs loadSceneDependencyAssetEventArgs = LoadSceneDependencyAssetEventArgs.Create(sceneAssetName, dependencyAssetName, loadedCount, totalCount, userData);
                m_LoadSceneDependencyAssetEventHandler(this, loadSceneDependencyAssetEventArgs);
                ReferencePool.Release(loadSceneDependencyAssetEventArgs);
            }
        }

        private void UnloadSceneSuccessCallback(string sceneAssetName, object userData)
        {
            m_UnloadingSceneAssetNames.Remove(sceneAssetName);
            m_LoadedSceneAssetNamesDic.Remove(sceneAssetName);
            if (m_UnloadSceneSuccessEventHandler != null)
            {
                UnloadSceneSuccessEventArgs unloadSceneSuccessEventArgs = UnloadSceneSuccessEventArgs.Create(sceneAssetName, userData);
                m_UnloadSceneSuccessEventHandler(this, unloadSceneSuccessEventArgs);
                ReferencePool.Release(unloadSceneSuccessEventArgs);
            }
        }

        private void UnloadSceneFailureCallback(string sceneAssetName, object userData)
        {
            m_UnloadingSceneAssetNames.Remove(sceneAssetName);
            if (m_UnloadSceneFailureEventHandler != null)
            {
                UnloadSceneFailureEventArgs unloadSceneFailureEventArgs = UnloadSceneFailureEventArgs.Create(sceneAssetName, userData);
                m_UnloadSceneFailureEventHandler(this, unloadSceneFailureEventArgs);
                ReferencePool.Release(unloadSceneFailureEventArgs);
                return;
            }

            throw new GameFrameworkException(Utility.Text.Format("Unload scene failure, scene asset name '{0}'.", sceneAssetName));
        }
      
    }
}

public class LoadSceneUpdateArgs
{
    public AsyncOperationHandle<SceneInstance> handle;
    public GameFramework.Scene.LoadSceneUpdateEventArgs args;
    public LoadSceneUpdateArgs(AsyncOperationHandle<SceneInstance> _Handler, string sceneAssetName, object userData)
    {
        handle = _Handler;
        args = GameFramework.Scene.LoadSceneUpdateEventArgs.Create(sceneAssetName, 0, userData);
    }

    public void Release()
    {
        GameFramework.ReferencePool.Release(args);
    }
}