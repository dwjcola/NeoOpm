using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using System.Threading.Tasks;
using System.Linq;
using Object = UnityEngine.Object;
using GameFramework;
using GameFramework.DataTable;
namespace GameFramework.AddressableResource
{
    public enum CacheUnloadTime
    {
        Immediately,
        CacheFinished,
        ChangeScene,
        ReLogin,
        Manual,
        Never
    }
    public class AsyncCache
    {
        public AsyncOperationHandle data;
        public CacheUnloadTime unloadTime;
        string resName;
        public AsyncCache(AsyncOperationHandle data, CacheUnloadTime unloadTime)
        {
            this.data = data;
            this.unloadTime = unloadTime;
        }
    }
    public class AASResourceManager
    {
        static AASResourceManager _Instance;
        public static AASResourceManager Instance
        {
            get                                                                                                                                                                                                                      
            {
                if (_Instance == null)
                {
                    _Instance = new AASResourceManager();
                }
                if (_Instance.PreLoadRes == null)
                {
                    var handle = Addressables.LoadAssetAsync<PreLoadResTable>(PreLoadResPath);
                    _Instance.CacheHandle(PreLoadResPath, handle,CacheUnloadTime.ReLogin);
                    _Instance.PreLoadRes = handle.WaitForCompletion();
                    //Addressables.Release(handle);
                    GameFrameworkLog.Info($"Load PreLoadRes {_Instance.PreLoadRes != null}" );
                }
                return _Instance;
            }
        }
        const string PathFormat = "Assets/Resource_MS/";
        const string AssetPath = "Assets";
        string FormatPath(string path)
        {
            path.Replace("\\", "/");
            if (!path.StartsWith(AssetPath))
            {
                path = PathFormat + path;
            }
            return path;
        }

        PreLoadResTable PreLoadRes;
        const string PreLoadResPath = "Assets/Resource_MS/SoTables/PreLoadRes.asset";
        /// <summary>
        /// 通过地址等加载的(Addressables.LoadAssetsAsync)都缓存在这里，避免交叉
        /// </summary>
        Dictionary<string, AsyncCache> cacheResDic = new Dictionary<string, AsyncCache>();

        ///<summary>
        ///通过标签等加载的(Addressables.LoadAssetsAsync)都缓存在这里，避免交叉
        ///</summary>>
        Dictionary<object, AsyncCache> cacheAssetsDic = new Dictionary<object, AsyncCache>();
        private bool HasCache(string assetName)
        {
            assetName = FormatPath(assetName);
            return cacheResDic.ContainsKey(assetName);
        }

        /// <summary>
        /// 如果资源需要缓存，则调用这个接口进行缓存
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="handle"></param>
        /// <param name="unloadTime"></param>
        public void CacheHandle(string assetName, AsyncOperationHandle handle, CacheUnloadTime unloadTime = CacheUnloadTime.Manual)
        {
            assetName = FormatPath(assetName);
            if (!HasCache(assetName))
            {
                cacheResDic.Add(assetName, new AsyncCache(handle, unloadTime));
            }
        }

        /// <summary>
        /// 按标签加载的不受表控制了
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="handle"></param>
        /// <param name="unloadTime"></param>
        public void CacheAssetsHandle(object assetName, AsyncOperationHandle handle, CacheUnloadTime unloadTime=CacheUnloadTime.Never)
        {
            if (!cacheAssetsDic.ContainsKey(assetName))
            {
                cacheAssetsDic.Add(assetName, new AsyncCache(handle, unloadTime));
            }
        }
        /// <summary>
        /// 通过资源路径卸载资源，移除缓存
        /// 非标签加载的资源
        /// </summary>
        /// <param name="assetName"></param>
        private bool UnloadResItem(string assetName)
        {
            assetName = FormatPath(assetName);
            AsyncCache cache;
            if (cacheResDic.TryGetValue(assetName, out cache))
            {
                UnLoadAssetHandle(cache?.data);
                cacheResDic.Remove(assetName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 通过句柄释放ab
        /// </summary>
        /// <param name="handle"></param>
        private void UnLoadAssetHandle(AsyncOperationHandle? handle)
        {
            if (handle == null)
                return;
            if (handle.Value.IsValid())
             Addressables.Release(handle.Value);
        }
       /// <summary>
       /// 通过资源名获取缓存的句柄
       /// </summary>
       /// <param name="assetName"></param>
       /// <returns></returns>
        private AsyncCache LoadAssetFromCache(string assetName)
        {
            assetName = FormatPath(assetName);
            AsyncCache cache = null;
            if (cacheResDic.TryGetValue(assetName, out cache))
            {
                if (cache == null || !cache.data.IsValid())
                {
                    cacheResDic.Remove(assetName);
                    UnLoadAssetHandle(cache?.data);
                    cache = null;
                }
            }
            return cache;
        }
       /// <summary>
       /// 异步加载资源，不进行句柄缓存，使用者拿到句柄后需要镜像释放，使用addressabel引用计数
       /// </summary>
       /// <param name="assetName"></param>
       /// <param name="onComplete"></param>
       /// <param name="onFailed"></param>
       /// <param name="autoUnload"></param>
       /// <typeparam name="T"></typeparam>
       /// <returns></returns>
       public AsyncOperationHandle<T> LoadAssetHandleAsyncNoCatch<T>(string assetName, Action<string, bool, T> onComplete = null, Action<Exception> onFailed = null, bool autoUnload = false) 
        {
            AsyncOperationHandle<T> loading = Addressables.LoadAssetAsync<T>(FormatPath(assetName));
            GameFrameworkLog.Info($"load asset {assetName} from AAS");
            loading.Completed += (result) =>
            {
                if (result.Status == AsyncOperationStatus.Succeeded)
                {
                    onComplete?.Invoke(assetName, result.Result != null && result.OperationException == null, result.Result);
                }
                else
                {
                    onFailed?.Invoke(result.OperationException);
                    GameFrameworkLog.Warning($"Load Asset{assetName} Failed From AAS");
                }
                if (autoUnload)
                    if (!UnloadResItem(assetName))
                        UnLoadAssetHandle(result);
            };
            return loading;
        }
       /// <summary>
       /// 异步加载资源——不缓存句柄
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="assetName"></param>
       /// <param name="onComplete">加载成功的回调</param>
       /// <param name="onFailed">加载失败的回调，在Completed的Status不为Succeeded时使用</param>
       /// <param name="autoUnload">为true时加载完成立即release handle,再对返回的handle调用addressable的release会有错误提示</param>
       /// <returns></returns>
       public async Task<T> LoadAssetAsyncNoCatch<T>(string assetName, Action<string, bool, T> onComplete = null, Action<Exception> onFailed = null, bool autoUnload = false)
       {
           return await LoadAssetHandleAsyncNoCatch(assetName, onComplete, onFailed, autoUnload).Task;
       }
        /// <summary>
        /// 异步加载优先使用缓存的handle，传入回调也可以，通过complete+=也可以
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="onComplete">加载成功的回调，不传也可以使用.Complete+=</param>
        /// <param name="onFailed">加载失败的回调，不传也可，在Completed的Status不为Succeeded时使用</param>
        /// <param name="autoUnload">为true时加载完成立即release handle,再对返回的handle调用addressable的release会有错误提示</param>
        /// <returns></returns>
        public AsyncOperationHandle<T> LoadAssetHandleAsync<T>(string assetName, Action<string, bool, T> onComplete = null, Action<Exception> onFailed = null, bool autoUnload = false) 
        {
            AsyncOperationHandle<T> loading;
            var cache = LoadAssetFromCache(assetName);
            if (cache != null)
            {
                GameFrameworkLog.Info($"load asset {assetName} from Cache");
                loading = cache.data.Convert<T>();

                loading.Completed += (result) =>
                {
                    if (result.Status == AsyncOperationStatus.Succeeded)
                    {
                        onComplete?.Invoke(assetName, result.Result != null && result.OperationException == null, result.Result);
                    }
                    else
                    {
                        onFailed?.Invoke(result.OperationException);
                        GameFrameworkLog.Warning($"Load Asset{assetName} Failed From Cache");
                    }
                    if (autoUnload)
                        if (!UnloadResItem(assetName))
                            UnLoadAssetHandle(result);
                };
                return loading;
            }
            loading = Addressables.LoadAssetAsync<T>(FormatPath(assetName));
            GameFrameworkLog.Info($"load asset {assetName} from AAS");
            CacheHandle(assetName, loading);
            loading.Completed += (result) =>
            {
                if (result.Status == AsyncOperationStatus.Succeeded)
                {
                    onComplete?.Invoke(assetName, result.Result != null && result.OperationException == null, result.Result);
                }
                else
                {
                    onFailed?.Invoke(result.OperationException);
                    GameFrameworkLog.Warning($"Load Asset{assetName} Failed From AAS");
                }
                if (autoUnload)
                    if (!UnloadResItem(assetName))
                        UnLoadAssetHandle(result);
            };
            return loading;
        }
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="onComplete">加载成功的回调</param>
        /// <param name="onFailed">加载失败的回调，在Completed的Status不为Succeeded时使用</param>
        /// <param name="autoUnload">为true时加载完成立即release handle,再对返回的handle调用addressable的release会有错误提示</param>
        /// <returns></returns>
        public async Task<T> LoadAssetAsync<T>(string assetName, Action<string, bool, T> onComplete = null, Action<Exception> onFailed = null, bool autoUnload = false)
        {
            return await LoadAssetHandleAsync(assetName, onComplete, onFailed, autoUnload).Task;
        }

        ///要实例化一个物体，要么直接使用addressable的异步实例化接口，可以从返回的handle里取到实例化后的物体，这个接口产生的引用计数会在切场景的时候自动清除
        ///或者在调用之后自行清除handle
        ///要么使用此脚本中的同步实例化接口，直接返回要实例化的资源，此接口会保存使用并缓存的handle
        ///或者先使用加载接口(也会保存并使用缓存),再使用返回的handle自行实例化资源

        /// <summary>
        /// 这个接口使用缓存实例化prefab，是同步接口,使用缓存的异步接口无法获取到实例化的物体
        /// 或者直接使用addressable的InstantiateAsync接口
        /// </summary>
        /// <returns></returns>
        public GameObject InstantiateAsset(string assetPath, InstantiationParameters parameters=default)
        {
            GameObject obj = LoadAssetSync<GameObject>(assetPath);
            if (obj == null)
            {
                GameFrameworkLog.Warning($"sync Instantiate {assetPath} fail,result is null");
                return null;
            }
            return parameters.Instantiate(obj);
        }
        /// <summary>
        /// 这个接口使用缓存实例化prefab，是同步接口,使用缓存的异步接口无法获取到实例化的物体
        /// 或者直接使用addressable的InstantiateAsync接口
        /// </summary>
        /// <returns></returns>
        public GameObject InstantiateAsset(string assetPath)
        {
            return InstantiateAsset(assetPath, new InstantiationParameters());
        }
        
        /// <summary>
        /// 也可以直接调用Addressables的InstantiateAsync接口，实例化的资源引用计数由addressable管理，在切换场景的时候会释放资源
        /// </summary>
        public AsyncOperationHandle<GameObject> InstantiateAsync(string assetpath, InstantiationParameters parameters=default, bool trackHandle = true)
        {
            assetpath = FormatPath(assetpath);
            return Addressables.InstantiateAsync(assetpath, parameters, trackHandle);
        }

        /// <summary>
        /// 在需要重复多次实例化同一个物体的时候可以使用这个接口,否则推荐直接使用addressable的InstantiateAsync
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="parameters">实例化参数</param>
        /// <param name="insAction">使用加载的缓存实例化，故只能通过回调的方式处理实例化的物体</param>
        /// <returns></returns>
        public void InstantiateUseCache(string assetPath, InstantiationParameters parameters=default, Action<GameObject> insAction=null)
        {
            LoadAssetHandleAsync<GameObject>(assetPath, (path, exception, result) =>
            {
                GameObject obj = parameters.Instantiate(result);
                insAction?.Invoke(obj);
            });
        }
        /// <summary>
        /// 同步方式，等待阻塞
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public T LoadAssetSync<T>(string assetName)
        {
            assetName = FormatPath(assetName);
            AsyncOperationHandle handle;
            if (cacheResDic.ContainsKey(assetName))
            {
                handle = cacheResDic[assetName].data/*.Convert<T>()*/;
            }
            else
            {
                handle = Addressables.LoadAssetAsync<T>(assetName);
                CacheHandle(assetName, handle);
            }
            return (T)handle.WaitForCompletion();
        }
        /// <summary>
         /// 释放小于等于当前级别的缓存资源——按地址加载的资源
         /// </summary>
         /// <param name="unloadTime"></param>
        private void ReleaseResCache(CacheUnloadTime unloadTime)
        {
            int maxCount = cacheResDic.Count;
            var keyList = cacheResDic.Keys.ToList();
            for (int i = 0; i < maxCount; i++)
            {
                AsyncCache cache = null;
                if (cacheResDic.TryGetValue(keyList[i], out cache))
                {
                    if (cache == null || cache.unloadTime <= unloadTime)
                    {
                        UnLoadAssetHandle(cache?.data);
                        cacheResDic.Remove(keyList[i]);
                    }
                }
            }
        }
        /// <summary>
        /// 释放小于等于当前级别的缓存资源——按标签加载的资源
        /// </summary>
        /// <param name="unloadTime"></param>
        private void ReleaseAssetsCache(CacheUnloadTime unloadTime)
        {
            int maxCount = cacheAssetsDic.Count;
            var keyList = cacheAssetsDic.Keys.ToList();
            for (int i = 0; i < maxCount; i++)
            {
                AsyncCache cache = null;
                if (cacheAssetsDic.TryGetValue(keyList[i], out cache))
                {
                    if (cache == null || cache.unloadTime <= unloadTime)
                    {
                        UnLoadAssetHandle(cache?.data);
                        cacheAssetsDic.Remove(keyList[i]);
                    }
                }
            }
        }
        /// <summary>
        /// 释放所有缓存资源，包括标签加载的
        /// </summary>
        public void UnLoadAllAssetCache()
        {
            foreach (var item in cacheResDic)
            {
                UnLoadAssetHandle(item.Value?.data);
            }
            foreach (var item in cacheAssetsDic)
            {
                UnLoadAssetHandle(item.Value?.data);
            }
            cacheResDic.Clear();
            cacheAssetsDic.Clear();
        }
        public void ReleaseCache(CacheUnloadTime unloadTime)
        {
            ReleaseResCache(unloadTime);
            ReleaseAssetsCache(unloadTime);
        }

        #region 按标签加载、按组加载
        /// <summary>
        /// 加载一个标签标定的所有assets
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lableName">标签名字</param>
        /// <param name="onComplete">针对该组每个单一资源的操作</param>
        /// <param name="onFailed">加载失败调用</param>
        /// <param name="autoUnload">为true时加载完成立即release handle,再对返回的handle调用addressable的release会有错误提示,返回的列表仍在</param>
        /// <returns></returns>
        public AsyncOperationHandle<IList<T>> LoadAssetsAsync<T>(string lableName, Action<string, bool, T> onComplete = null, Action<Exception> onFailed = null, bool autoUnload = false)
        {
            AsyncOperationHandle<IList<T>> loading;
            if (cacheAssetsDic.ContainsKey(lableName))
            {
                loading = cacheAssetsDic[lableName].data.Convert<IList<T>>();
                if (loading.IsValid())
                {
                    loading.Completed += (h) =>
                    {
                        if (h.Status == AsyncOperationStatus.Succeeded)
                        {
                            foreach (var item in h.Result)
                            {
                                onComplete("", false, item);
                            }
                        }
                    };
                    return loading;
                }
                else { ReleaseAssetsCache(lableName); }
            }
            loading = Addressables.LoadAssetsAsync<T>(lableName, (result) => { onComplete?.Invoke(lableName, false, result); });
            CacheAssetsHandle(lableName, loading);
            
            loading.Completed += (result) =>
            {
                if (result.Status != AsyncOperationStatus.Succeeded)
                {
                    onFailed?.Invoke(result.OperationException);
                }
                if (autoUnload)
                { 
                    if (ReleaseAssetsCache(lableName))
                    {
                        UnLoadAssetHandle(result);
                    }
                }
            };

            return loading;
        }
        /// <summary>
        /// 完全使用addressable的LoadAssetsAsync，不经缓存handle,autoUnload为false需要手动释放返回的handle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">一个addressable地址的列表或者标签列表，受MergeMode制约</param>
        /// <param name="onComplete">加载完成对返回列表中每一个元素操作的回调</param>
        /// <param name="mode">mergemode</param>
        /// <param name="autoUnload">为false需要手动释放,为true时加载完成立即release handle,再对返回的handle调用addressable的release会有错误提示</param>
        /// <returns></returns>
        public AsyncOperationHandle<IList<T>> LoadAssetsAsync<T>(IEnumerable keys, Action<string, bool, T> onComplete = null, Addressables.MergeMode mode = Addressables.MergeMode.Union, bool autoUnload = false)
        {
            AsyncOperationHandle<IList<T>> handle;
            if (cacheAssetsDic.ContainsKey(keys))
            {
                handle = cacheAssetsDic[keys].data.Convert<IList<T>>();
                if (handle.IsValid())
                {
                    handle.Completed += (h) =>
                    {
                        if (h.Status == AsyncOperationStatus.Succeeded)
                        {
                            foreach (var item in h.Result)
                            {
                                onComplete("", false, item);
                            }
                        }
                    };
                    return handle;
                }
                else
                {
                    ReleaseAssetsCache(keys);
                }
            }
            handle = Addressables.LoadAssetsAsync<T>(keys, (result) => { onComplete("", false, result); }, mode, true);
            CacheAssetsHandle(keys, handle);
            handle.Completed += (h) =>
            {
                if (autoUnload)
                {
                    if (ReleaseAssetsCache(keys))
                    {
                        UnLoadAssetHandle(h);
                    }
                }
            };
            return handle;
        }
       
        /// <summary>
        /// 释放标签加载的缓存资源
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        private bool ReleaseAssetsCache(object keys)
        {
            AsyncCache cache;
            if (cacheAssetsDic.TryGetValue(keys, out cache))
            {
                UnLoadAssetHandle(cache?.data);
                cacheAssetsDic.Remove(keys);
                return true;
            }
            return false;
        }
        #endregion
    }
}