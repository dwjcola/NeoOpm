using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Resource;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace GameFramework.AddressableResource
{
    /// <summary>
    /// 资源管理器。
    /// </summary>
    internal sealed partial class AddressableResourceManager : GameFrameworkModule, IAddressableResourceManager
    {
        /// <summary>
        /// 初始化资源管理器的新实例。
        /// </summary>
        public AddressableResourceManager()
        {
           
        }

        internal override void Shutdown()
        {
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get { return 70; }
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        public void LoadAsset<TObject>(string assetName, LoadAssetCallbacks loadAssetCallbacks)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }

            LoadAsset<TObject>(assetName, loadAssetCallbacks, null);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset<TObject>(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }
            AASResourceManager.Instance.LoadAssetHandleAsync<TObject>(assetName).Completed += (handle) => {
                var asset = handle.Result;
                if (asset != null)
                {
                    loadAssetCallbacks.LoadAssetSuccessCallback?.Invoke(assetName, asset, 0, userData);
                }
                else
                {
                    loadAssetCallbacks.LoadAssetFailureCallback?.Invoke(assetName, LoadResourceStatus.AssetError, "Can not load this asset from asset database.", userData);
                }
            };
        }
        public async Task<TObject> GetJsonTable<TObject>(string jsonName)
        {
            return await AASResourceManager.Instance.LoadAssetAsync<TObject>(string.Format("SoTables/{0}.asset", jsonName));
        }
        public async Task<Sprite> GetSprite(string atlas, string spname)
        {
            return await AASResourceManager.Instance.LoadAssetAsync<Sprite>(atlas.Trim() + "[" + spname + "]");
        }
        public async Task<Texture2D> GetTexture(string textureName)
        {
            return await AASResourceManager.Instance.LoadAssetAsync<Texture2D>("Assets/Resource_MS/UI/UITexture/"+textureName);
        }
        public async Task<Sprite[]> GetSprites(string atlas)
        {
            return await AASResourceManager.Instance.LoadAssetAsync<Sprite[]>(atlas.Trim());
        }
        public async Task<Sprite> GetAtlasSprite(string atlas, string spname)
        {
            SpriteAtlas a= await AASResourceManager.Instance.LoadAssetAsync<SpriteAtlas>(atlas.Trim());
            return a.GetSprite(spname);
        }
        public async Task<Material> GetMaterial(string path)
        {
            return await AASResourceManager.Instance.LoadAssetAsync<Material>(path);
        }
        /// <summary>
        /// 加载资源 同步接口，优先使用缓存实例化
        /// </summary>
        /// <param name="assetKey">资源路径名</param>
        /// <returns></returns>
        public GameObject InstantiateAsset(object assetKey)
        {
            return AASResourceManager.Instance.InstantiateAsset(assetKey.ToString()); // Instantiate((Object)uiFormAsset);
        }

        public AsyncOperationHandle<GameObject> InstantiateAssetAsync(object assetKey)
        {
            return AASResourceManager.Instance.InstantiateAsync(assetKey.ToString()); // Instantiate((Object)uiFormAsset);
        }
        /// <summary>
        /// 这个异步接口使用缓存实例化
        /// </summary>
        /// <param name="assetKey"></param>
        /// <param name="parameters"></param>
        /// <param name="insAction"></param>
        public void InstantiateUseCache(object assetKey, InstantiationParameters parameters=default, Action<GameObject> insAction=null)
        {
        
            AASResourceManager.Instance.InstantiateUseCache(assetKey.ToString(), parameters, insAction);
        }
        /// <summary>
        /// 删除加载资源
        /// </summary>
        /// <param name="assetKey">资源路径名</param>
        /// <returns></returns>
        public void ReleaseInstantiateAsset(GameObject asset)
        {
            // Instantiate((Object)uiFormAsset);
            if (!Addressables.ReleaseInstance(asset))
            {
                GameObject.Destroy(asset);
            }
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            //Addressables.Release(asset);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        public void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks)
        {
            LoadScene(sceneAssetName, loadSceneCallbacks, null);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="sceneAssetName">要加载场景资源的名称。</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadScene(string sceneAssetName, LoadSceneCallbacks loadSceneCallbacks, object userData)
        {
            Addressables.LoadSceneAsync(sceneAssetName, LoadSceneMode.Additive).Completed += (handle) =>
            {
                var asset = handle.Result;
                // asset.ActivateAsync().completed += (asyncOperation) => {
                    loadSceneCallbacks.LoadSceneSuccessCallback?.Invoke(sceneAssetName, asset, 0, userData);
                // };
            };
        }

        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        public void UnloadScene(string sceneAssetName, SceneInstance sceneAsset, UnloadSceneCallbacks unloadSceneCallbacks)
        {
            UnloadScene(sceneAssetName, sceneAsset, unloadSceneCallbacks, null);
        }
        
        /// <summary>
        /// 异步卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">要卸载场景资源的名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void UnloadScene(string sceneAssetName, SceneInstance sceneAsset, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            Addressables.UnloadSceneAsync(sceneAsset).Completed += (handle) =>
            {
                var asset = handle.Result;
                unloadSceneCallbacks.UnloadSceneSuccessCallback?.Invoke(sceneAssetName, userData);
            };
        }

        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(string sceneName, LoadSceneMode mode)
        {
            return Addressables.LoadSceneAsync(sceneName, mode);
        }

        public AsyncOperationHandle<T> LoadAssetAsync<T>(string assetName, bool autoRelease=false)
        {
            return AASResourceManager.Instance.LoadAssetHandleAsync<T>(assetName,null,null, autoRelease);
        }
      
        public void LoadAssetAsync<TObject>(string assetName, Action<TObject> callback)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameFrameworkException("Asset name is invalid.");
            }

            if (callback == null)
            {
                throw new GameFrameworkException("Load asset callbacks is invalid.");
            }
            AASResourceManager.Instance.LoadAssetHandleAsync<TObject>(assetName).Completed += (AsyncOperationHandle<TObject> handle) =>
            {
                var asset = handle.Result;
                if (asset != null)
                {
                    callback.Invoke(asset);
                }
                else
                {
                    GameFrameworkLog.Error("Can not load "+assetName+" from asset database.");
                }
            };
        }

        public Task<T> LoadAssetAsync22<T>(string assetName)
        {
            TaskCompletionSource<T> r = new TaskCompletionSource<T>();
            AASResourceManager.Instance.LoadAssetHandleAsync<T>(assetName).Completed += (ret)=> { r.SetResult(ret.Result); };   
            return r.Task;
        }
        
        public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IEnumerable keys, Action<TObject> callback, Addressables.MergeMode mode)
        {
            return AASResourceManager.Instance.LoadAssetsAsync<TObject>(keys, (name,exception,result)=> { callback(result); }, mode);
        }

        public void Release<TObject>(AsyncOperationHandle<TObject> handle)
        {
           // Addressables.Release(handle);
        }
        public Task<IList<T>> LoadAssetsAsync<T>(string lable,Action<T> action=null,bool autoRelease=false)
        {
            return AASResourceManager.Instance.LoadAssetsAsync<T>(lable, (name, exception, result) => { action(result); },null, autoRelease).Task;
        }

        public TObject LoadAssetSync<TObject>(string assetName)
        {
            TObject obj= AASResourceManager.Instance.LoadAssetSync<TObject>(assetName);
            return obj;
        }
    }
}